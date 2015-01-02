using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using EnvDTE;
using EnvDTE80;

namespace DelegateAS.SmartResource
{

    /// <summary>
    /// Main class for methods used by the extension
    /// </summary>
    internal class Main
    {
        /*
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private uint WM_CUT = 0x300;
        private uint WM_COPY = 0x301;
        private uint WM_PASTE = 0x302;
        private uint WM_CLEAR = 0x303;
        */
        private OptionsDialogPage _options;
        private DTE2 _applicationObject;
         
        // Main entry method
        internal void ReplaceStringWithResource(DTE2 applicationObject, OptionsDialogPage options)
        {
            _options = options;

            _applicationObject = applicationObject;

            string selectedText = null;
            bool isQuotedText = false;
            bool isQuotesContainedInSelect = false;
            bool isInCustomMode = false;
            TextDocument vsTextDoc = null;

            TextSelection sel = null;
            int absCharPosStart = -1;
            int selLength = 0;

            if (applicationObject.ActiveDocument == null || applicationObject.ActiveDocument.Selection == null)
            {
                isInCustomMode = true;
                //var ret = SendMessage( new IntPtr( _applicationObject.ActiveWindow.HWnd ), WM_COPY, IntPtr.Zero, IntPtr.Zero);

                selectedText = Clipboard.GetText();

                DialogResult result = MessageBox.Show(string.Format("Notice! A bit of help required on designers\r\n\r\n" +
                                                                    "You need to do the copy/paste manually.\r\n\r\n" +
                                                                    "Continue with clipboard string \"{0}\"?",
                    selectedText),
                    "Smart Resources - Confirm", MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                sel = (TextSelection)applicationObject.ActiveDocument.Selection;

                if (sel.IsEmpty || string.IsNullOrEmpty(sel.Text))
                {
                    MessageBox.Show(Resources.Main_ReplaceStringWithResource_SelectSomeText,
                        Resources.DialogBox_DefaultTitle, MessageBoxButtons.OK);
                    return;
                }

                vsTextDoc = applicationObject.ActiveDocument.Object("TextDocument") as TextDocument;
                Debug.Assert(vsTextDoc != null, "no text doc found");

                absCharPosStart = sel.TextRanges.Item(1).StartPoint.AbsoluteCharOffset;
                selLength = sel.Text.Length;


                // Check for quotes
                // ----------------

                // Expand selection to see if there is any quotes
                EditPoint2 s = ((EditPoint2) (sel.TextRanges.Item(1).StartPoint));
                EditPoint2 e = ((EditPoint2) (sel.TextRanges.Item(1).EndPoint));

                s.CharLeft();
                if (s.GetText(1) == "\"")
                {
                    //expand left
                    absCharPosStart--;
                    selLength++;
                }
                if (e.GetText(1) == "\"")
                {
                    //expand right
                    selLength++;
                }

                s.MoveToAbsoluteOffset(absCharPosStart);
                selectedText = s.GetText(selLength);
            }

            if (Regex.IsMatch(selectedText, "^\\s*\".*\"\\s*$"))
            {
                isQuotedText = true;
                isQuotesContainedInSelect = true;
                selectedText = selectedText.Trim('"');
            }


            string sourceFile = isInCustomMode ? "" : applicationObject.ActiveDocument.FullName;
            Project project = applicationObject.ActiveWindow.Project;
            if (project.Properties == null)
            {
                MessageBox.Show( Resources.Main_ReplaceStringWithResource_NoProjectProperties );
                return;
            }

            ProjectItem resXfile = FindResXFile(project);
            if (resXfile == null)
            {
                MessageBox.Show(Resources.Main_ReplaceStringWithResource_NoResXFile);
                return; 
            }
            CheckOut(resXfile);

            //find any existing keys
            IList<string> existingKeys = FindKeys(resXfile, selectedText);
            // Choose key
            string msg;
            if (existingKeys.Count > 0)
            {
                msg = "Choose an existing key, or type a new\r\n\r\n";
                for (var i = 0; i <= existingKeys.Count - 1; i++)
                {
                    msg += i + ": " + existingKeys[i] + "\r\n";
                }
            }
            else
            {
                msg = "No existing key exists with that value";
            }
            // Either add a new value or use existing
            string key = Microsoft.VisualBasic.Interaction.InputBox(msg, "File " + resXfile.Name);

            int labelIdx;
            if (int.TryParse(key, out labelIdx))
            {
                key = existingKeys[labelIdx];
            }
            else if (string.IsNullOrEmpty(key))
            {
                // Abort 
                return;
            }
            else
            {
                //New key

                //Handle spaces
                key = key.Replace(" ", "_");
                if (KeyAlreadyExists(resXfile, key))
                {
                    MessageBox.Show(string.Format("Key {0} already exists in the resource file. Choose another", key));
                    return;
                }

                // New key, store
                if (!StoreKey(resXfile, key, selectedText, sourceFile))
                {
                    return;
                }
            }
            // And finally insert the new key. 
            // Handle filetypes and insert it in various ways (VB, CS, AS?X)
            string activePattern;
            if (Regex.IsMatch(sourceFile, ".*\\.as.x$"))
            {
                // aspx like file
                activePattern = _options.AsxxPattern;
            }
            else if (Regex.IsMatch(sourceFile, ".*\\.cs$"))
            {
                activePattern = _options.CsPattern;
            }
            else if (Regex.IsMatch(sourceFile, ".*\\.vb$"))
            {
                activePattern = _options.VbPattern;
            }
            else if (isQuotedText)
            {
                activePattern = "\"" + _options.XmlPattern + "\"";
            }
            else
            {
                activePattern = _options.XmlPattern;
            }

            string replaceText = string.Format(activePattern, Path.GetFileNameWithoutExtension(resXfile.Name), key, Path.GetFileNameWithoutExtension(resXfile.Name).Replace(".","_"));

            if (isInCustomMode)
            {
                if (isQuotesContainedInSelect)
                {
                    replaceText = "\"" + replaceText + "\"";
                }
                Clipboard.SetText(replaceText);
                MessageBox.Show("Please paste the replace pattern now.", 
                    Resources.DialogBox_DefaultTitle,
                    MessageBoxButtons.OK);
            }
            else
            {
                EditPoint2 start = (EditPoint2) vsTextDoc.CreateEditPoint();
                start.MoveToAbsoluteOffset( absCharPosStart );
                // Kill the current text and optionally any quotes (are included in pattern if needed)
                start.Delete(selLength);
                start.Insert(replaceText);

            }

            //Replace completed :-)
        }


        private bool KeyAlreadyExists(ProjectItem resXfile, string key)
        {
            string filename = resXfile.FileNames[0];
            XDocument data = XDocument.Load(filename);
            string xpath = "//data[@name='" + key.Replace("'", "") + "']";
            IEnumerable<XElement> keys = data.XPathSelectElements(xpath);
            return keys.Any();
        }

        // Get the keys from resx file with specified value
        IList<string> FindKeys(ProjectItem resXFile, string value)
        {
            if (resXFile.IsDirty)
            {
                resXFile.Save();
            }
            string filename = resXFile.FileNames[0];
            XDocument data = XDocument.Load(filename);
            string xpath = "//data[./value='" + value.Replace("'", "") + "']";
            IEnumerable<XElement> keys = data.XPathSelectElements(xpath);
            return keys.ToList().Select( x=> x.Attribute("name").Value).ToList();
        }

        // Store a new key in resx file
        bool StoreKey(ProjectItem resXFile, string key, string value, string sourceFile)
        {
            if (!resXFile.Saved)
            {
                resXFile.Save();
            }

            string filename = resXFile.FileNames[0];
            FileInfo finfo = new FileInfo(filename);
            if (finfo.IsReadOnly)
            {
                MessageBox.Show(string.Format("Oops sorry, resource file ({0}) is read only. \r\n\r\ndid you remember to check it out?", resXFile.Name), Resources.DialogBox_DefaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            XDocument data = XDocument.Load(filename);
            XElement newKey = new XElement("data", new XAttribute("name", key), new XElement("value", value), new XElement("comment", "Used in " + sourceFile));

            Debug.Assert(data.Root != null, "data.Root != null");
            data.Root.Add(newKey);
            data.Save(filename);
            return true;
        }

        // Locate the current resx file to work with.
        //  
        //  Find the target name (from project properties) and assume that the resx file ends with that
        //  Note: Does only handle default language
        ProjectItem FindResXFile(Project project)
        {
            string target = GetPropertyValue(project, "AssemblyName");
            ProjectItem item = FindItem(project.ProjectItems, target + ".resx", false);

            //Default to Resources.resx
            if (item == null)
            {
                item = FindItem(project.ProjectItems, "Resources.resx", false);
            }

            //Default to the one resx file if only one exists
            if (item == null)
            {
                item = FindItem(project.ProjectItems, ".resx", true);
            }

            return item;
        }

        //  Get a specific property value from the project properties
        string GetPropertyValue(Project project, string key)
        {
            foreach (Property prop in project.Properties)
            {
                if (prop.Name == key)
                {
                    return ((string)prop.Value);
                }
            }
            throw new ApplicationException("Property " + key + " not found");
        }

        // Locate a given item by name in the current project
        //  Uses CONTAINS on the name, so be carefull
        //
        private ProjectItem FindItem(ProjectItems projectItems, string name, bool errorOnMultiple)
        {
            ProjectItem firstFound = null;

            if (projectItems != null)
            {
                for (var i = 1; (i <= projectItems.Count); i++)
                {
                    var projectItem = projectItems.Item(i);
                    if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFile)
                    {
                        // If this is a c# file             
                        if (projectItem.Name.ToLower().Contains(name.ToLower()))
                        {
                            // Set flag to true if file is already open             
                            if (errorOnMultiple)
                            {
                                if (firstFound != null)
                                {
                                    throw new ApplicationException(
                                        "Multiple resx files found. Unable to decide (First priority is assemblyname.resx, then Resources.resx, then *.resx)");
                                }

                                firstFound = projectItem;
                            }
                            else
                            {
                                return projectItem;
                            }
                        }
                    }
                    // Be sure to apply RemoveAndSort on all of the ProjectItems.         
                    var item = FindItem(projectItem.ProjectItems, name, errorOnMultiple);
                    if (item != null)
                    {
                        if (errorOnMultiple)
                        {
                            if (firstFound != null)
                            {
                                throw new ApplicationException(
                                    "Multiple resx files found. Unable to decide (First priority is assemblyname.resx, then Resources.resx, then *.resx)");
                            }

                            firstFound = projectItem;
                        }
                        else
                        {
                            return item;
                        }
                    }
                }
            }
            return firstFound;
        }

        /// <summary>
        /// Checkout the file if required
        /// </summary>
        /// <param name="resXfile"></param>
        private void CheckOut(ProjectItem resXfile)
        {
            string filename = resXfile.FileNames[0];
            SourceControl sc = _applicationObject.SourceControl;
            if (sc != null)
            {
                if (sc.IsItemUnderSCC(filename) && !sc.IsItemCheckedOut(filename))
                {
                    if (!sc.CheckOutItem(filename))
                    {
                        MessageBox.Show(Resources.Main_CheckOut_Failed + filename, Resources.DialogBox_DefaultTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}
