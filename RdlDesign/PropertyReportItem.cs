/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyReportItem - The ReportItem Properties
    /// </summary>
    //[PropertyTab(typeof(PropertyTableTab), PropertyTabScope.Component),
    //  DefaultPropertyAttribute("Name")]
    [DefaultPropertyAttribute("Name")]
    internal class PropertyReportItem : ICustomTypeDescriptor, IReportItem 
    {
   		private DesignXmlDraw _Draw;
        private DesignCtl _DesignCtl;
        List<XmlNode> _RIs;
        private XmlNode _node;
        private XmlNode _tnode = null;
        private XmlNode _mnode = null;
        private object _custom = null;
        bool bGroup = false;
        bool InTable = false;
        bool InMatrix = false;
        bool bCustom = false;

        internal PropertyReportItem(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris)
        {
            _Draw = d;
            _DesignCtl = dc; 
            _RIs = ris;
            _node = _RIs[0];        // the first node is the model for all of the nodes
            if (_RIs.Count > 1)
                bGroup = true;
            else
                bCustom = _node.Name == "CustomReportItem";
            
            InTable = _Draw.InTable(_node);
            if (InTable)
                _tnode =  _Draw.GetTableFromReportItem(_node);

            InMatrix = _Draw.InMatrix(_node);
            if (InMatrix)
                _mnode = _Draw.GetMatrixFromReportItem(_node);
        }

        internal XmlNode TableNode
        {
            get { return _tnode; }
        }
        internal DesignXmlDraw Draw
        {
            get { return _Draw; }
        }
        internal DesignCtl DesignCtl
        {
            get { return _DesignCtl; }
        }

        internal XmlNode Node
        {
            get { return _node; }
        }

        internal List<XmlNode> Nodes
        {
            get { return _RIs; }
        }
 
        #region Design
        [CategoryAttribute("Design"),
           ParenthesizePropertyName(true),
           DescriptionAttribute("The unique name of the report item.")]
        public string Name
        {
            get
            {
                return GetName(_node);
            }
            set
            {
                SetName(_node, value);
            }
        }
#endregion
        
        #region Style
        [CategoryAttribute("Style"), 
          DescriptionAttribute("Defines the border of the report item.")]
        public PropertyBorder Border
        {
            get
            {
                return new PropertyBorder(this);
            }
        }
        [CategoryAttribute("Style"),
           DescriptionAttribute("Controls the padding expressions")]
        public PropertyPadding Padding
        {
            get
            {
                return new PropertyPadding(this);
            }
        }
        [CategoryAttribute("Style"),
           DescriptionAttribute("Controls the background expressions")]
        public PropertyBackground Background
        {
            get
            {
                return new PropertyBackground(this);
            }
        }
        #endregion
        #region Behavior
        [CategoryAttribute("Behavior"),
           DescriptionAttribute("Defines a hyperlink, bookmark link, or drillthrough action for the report item.")]
        public PropertyAction Action
        {
            get
            {
                return new PropertyAction(this);
            }
        }
        [CategoryAttribute("Behavior"),
           DescriptionAttribute("In PDFs, bookmarks are created for each instance of the report item.  For example, putting a bookmark on a report item in a group header will generate a list of the groups in the report.")]
        public PropertyExpr Bookmark
        {
            get
            {
                string ex = GetValue("Bookmark", "");
                return new PropertyExpr(ex);
            }
            set
            {
                SetValue("Bookmark", value.Expression);
            }

        }
        [CategoryAttribute("Behavior"),
           DescriptionAttribute("A ToolTip provides a label that can be used by a renderer.  For example, the Viewer and PDF renderers use this to popup the specified text when the mouse is over the report item.")]
        public PropertyExpr ToolTip
        {
            get 
            {
                string ex = GetValue("ToolTip", "");
                return new PropertyExpr(ex); 
            }
            set
            {
                SetValue("ToolTip", value.Expression);
            }

        }
        [CategoryAttribute("Behavior"),
          DescriptionAttribute("Defines the visibility of the item.")]
        public PropertyVisibility Visibility
        {
            get
            {
                return new PropertyVisibility(this);
            }
        }
        #endregion
         
        #region XML
        [CategoryAttribute("XML"),
   DescriptionAttribute("The name to use for the element or attribute when exporting to XML.")]
        public string DataElementName
        {
            get
            {
                return GetValue("DataElementName", "");
            }
            set
            {
                SetValue("DataElementName", value);
            }
        }
        [CategoryAttribute("XML"),
   DescriptionAttribute("When rendering XML determines how or whether the item appears in the XML.")]
        public DataElementOutputEnum DataElementOutput
        {
            get
            {
                string v = GetValue("DataElementOutput", "Auto");
                return fyiReporting.RDL.DataElementOutput.GetStyle(v);
            }
            set
            {
                SetValue("DataElementOutput", value.ToString());
            }
        }

        [CategoryAttribute("Layout"),
   DescriptionAttribute("Drawing order of the report item.")]
        public int ZIndex
        {
            get
            {
                string v = GetValue("ZIndex", "0");

                try
                {
                    return Convert.ToInt32(v);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("ZIndex must be greater or equal to 0.");
                SetValue("ZIndex", value.ToString());
            }
        }

        [CategoryAttribute("Layout"),
   DescriptionAttribute("Report item can span multiple columns.")]
        public int ColumnSpan
        {
            get
            {
                XmlNode ris = _node.ParentNode;
                XmlNode tcell = ris.ParentNode;
                if (tcell == null)
                    return 1;
                string colspan = _Draw.GetElementValue(tcell, "ColSpan", "1");
                return Convert.ToInt32(colspan);
            }
            set
            {
                XmlNode ris = _node.ParentNode;
                XmlNode tcell = ris.ParentNode;
                if (tcell != null && tcell.Name == "TableCell")
                {	// SetTableCellColSpan does all the heavy lifting; 
                    //    ie making sure the # of columns continue to match
                    _DesignCtl.StartUndoGroup("Column Span change");
                    _Draw.SetTableCellColSpan(tcell, value.ToString());
                    _DesignCtl.EndUndoGroup(true);
                    _DesignCtl.SignalReportChanged();
                    _Draw.Invalidate();
                }
            }
        }

        [CategoryAttribute("Layout"),
   DescriptionAttribute("Height and width of the report item.")]
        public PropertySize Size
        {
            get
            {
                string w = GetValue("Width", "");
                string h = GetValue("Height", "");
                return new PropertySize(this, h, w);
            }
        }
        [CategoryAttribute("Layout"),
   DescriptionAttribute("Location of the report item.")]
        public PropertyLocation Location
        {
            get
            {
                string x = GetValue("Left", "");
                string y = GetValue("Top", "");
                return new PropertyLocation(this, x, y);
            }
        }

        #endregion

        #region Table
        [CategoryAttribute("Table"), 
            DescriptionAttribute("Table report item properties.")]
        public PropertyTable Table
        {
            get
            {
                if (_tnode == null)
                    return null;
                List<XmlNode> ris = new List<XmlNode>();
                ris.Add(_tnode);

                return new PropertyTable(_Draw, _DesignCtl, ris);
            }
        }
        #endregion

#region Matrix
        [CategoryAttribute("Matrix"),
            DescriptionAttribute("Matrix report item properties.")]
        public PropertyMatrix Matrix
        {
            get
            {
                if (_mnode == null)
                    return null;
                List<XmlNode> ris = new List<XmlNode>();
                ris.Add(_mnode);

                return new PropertyMatrix(_Draw, _DesignCtl, ris);
            }
        }
        #endregion


        internal bool IsExpression(string e)
        {
            if (e == null)
                return false;
            string t = e.Trim();
            if (t.Length == 0)
                return false;

            return t[0] == '=';
        }

        internal string GetName(XmlNode node)
        {
            if (node == null)
                return "";
            XmlAttribute xa = node.Attributes["Name"];
            return xa == null ? "" : xa.Value;
        }

        internal void SetName(XmlNode node, string name)
        {
            if (node == null)
                return;

            string n = name.Trim();
            string nerr = _Draw.NameError(node, n);
            if (nerr != null)
            {
                throw new ApplicationException(nerr);
            }

            _DesignCtl.StartUndoGroup("Name change");
            _Draw.SetName(node, n);
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal string GetWithList(string def, params string[] names)
        {
            return GetWithList(_node, def, names);
        }

        internal string GetWithList(XmlNode n, string def, params string[] names)
        {
            foreach (string nm in names)
            {
                n = _Draw.GetNamedChildNode(n, nm);
                if (n == null)
                    return def;
            }
            return n.InnerText;
        }

        internal void RemoveWithList(params string[] names)
        {
            // build the name of the change
            StringBuilder sb = new StringBuilder();
            foreach (string s in names)
            {
                sb.Append(s);
                sb.Append(' ');
            }
            sb.Append("change");
            _DesignCtl.StartUndoGroup(sb.ToString());

            // loop thru all the selected nodes to make the change
            foreach (XmlNode n in _RIs)
            {
                RemoveWithList(n, names);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        private void RemoveWithList(XmlNode n, params string[] names)
        {
            foreach (string nm in names)
            {
                n = _Draw.GetNamedChildNode(n, nm);
                if (n == null)
                    return;
            }
            XmlNode p = n.ParentNode;
            _Draw.RemoveElement(p, names[names.Length-1]);
        }

        internal void SetWithList(string v, params string[] names)
        {
            // build the name of the change
            StringBuilder sb = new StringBuilder();
            foreach (string s in names)
            {
                sb.Append(s);
                sb.Append(' ');
            }
            sb.Append("change");
            _DesignCtl.StartUndoGroup(sb.ToString());

            // loop thru all the selected nodes to make the change
            foreach (XmlNode n in _RIs)
            {
                SetWithList(n, v, names);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        private void SetWithList(XmlNode n, string v, params string[] names)
        {
            foreach (string nm in names)
            {
                n = _Draw.GetCreateNamedChildNode(n, nm);
            }
            
            n.InnerText = v;
        }

        internal string GetValue(string l, string v)
        {
            return _Draw.GetElementValue(_node, l, v);
        }
        internal string GetValue(string l1, string l2, string v)
        {
            XmlNode sNode = _Draw.GetNamedChildNode(_node, l1);
            if (sNode == null)
                return v;
            return _Draw.GetElementValue(sNode, l2, v);
        }
        internal string GetValue(string l1, string l2, string l3, string v)
        {
            XmlNode sNode = _Draw.GetNamedChildNode(_node, l1);
            if (sNode == null)
                return v;
            sNode = _Draw.GetNamedChildNode(sNode, l2);
            if (sNode == null)
                return v;
            return _Draw.GetElementValue(sNode, l3, v);
        }

        internal void SetValue(string l, bool b)
        {
            SetValue(l, b ? "true" : "false");
        }

        internal void SetValue(string l, string v)
        {
            _DesignCtl.StartUndoGroup(l+ " change");
            foreach (XmlNode n in _RIs)
            {
                _Draw.SetElement(n, l, v);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal void SetValue(string l1, string l2, string v)
        {
            _DesignCtl.StartUndoGroup(string.Format("{0} {1} change", l1, l2));
            foreach (XmlNode n in _RIs)
            {
                XmlNode lNode = _Draw.GetCreateNamedChildNode(n, l1);
                _Draw.SetElement(lNode, l2, v);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal void SetValue(string l1, string l2, string l3, string v)
        {
            _DesignCtl.StartUndoGroup(string.Format("{0} {1} change", l1, l2));
            foreach (XmlNode n in _RIs)
            {
                XmlNode aNode = _Draw.GetCreateNamedChildNode(n, l1);
                XmlNode lNode = _Draw.GetCreateNamedChildNode(aNode, l2);
                _Draw.SetElement(lNode, l3, v);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal void RemoveValue(string l1)
        {
            _DesignCtl.StartUndoGroup(string.Format("{0} change", l1));
            foreach (XmlNode n in _RIs)
            {
               _Draw.RemoveElement(n, l1);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal void RemoveValue(string l1, string l2)
        {
            _DesignCtl.StartUndoGroup(string.Format("{0} {1} change", l1, l2));
            foreach (XmlNode n in _RIs)
            {
                XmlNode lNode = _Draw.GetNamedChildNode(n, l1);
                if (lNode != null)
                    _Draw.RemoveElement(lNode, l2);
            }
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        #region ICustomTypeDescriptor Members
        // Implementation of ICustomTypeDescriptor: 

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            // Get the collection of properties
            PropertyDescriptorCollection bProps = 
                                              TypeDescriptor.GetProperties(this, true);
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(null);

            // For each property use a property descriptor of our own that is able to 
            // be globalized
            foreach( PropertyDescriptor p in bProps )
            {
                if (p.Name == "Name" && bGroup)
                    continue;
                
                if (InTable || InMatrix)
                {   // Never show size or location when in a table or matrix
                    if (p.Name == "Size" || p.Name == "Location")
                        continue;
                }
                if (!InTable)
                {   // Only show table related properties when in a table
                    if (p.Category == "Table")
                        continue;
                    if (p.Name == "ColumnSpan")
                        continue;
                }
                if (!InMatrix)
                {   // Only show matrix related properties when in a matrix
                    if (p.Category == "Matrix")
                        continue;
                }
                // create our custom property descriptor and add it to the collection
                pdc.Add(p);
            }

            if (bCustom)
                SetCustomReportItem(pdc);
            
            return pdc;
        }
        
        private void SetCustomReportItem(PropertyDescriptorCollection pdc)
        {
            ICustomReportItem cri = null;
            try
            {
                string t = _Draw.GetElementValue(this.Node, "Type", "");

                cri = RdlEngineConfig.CreateCustomReportItem(t);

                _custom = cri.GetPropertiesInstance(_Draw.GetNamedChildNode(this.Node, "CustomProperties"));
                TypeDescriptor.CreateAssociation(this, _custom);

                PropertyDescriptorCollection bProps =
                                  TypeDescriptor.GetProperties(_custom, true);

                foreach (PropertyDescriptor p in bProps)
                {
                    // create our custom property descriptor and add it to the collection
                    
                    pdc.Add(p);
                }
                
            }
            catch
            {
            }
            finally
            {
                if (cri != null)
                    cri.Dispose();
            }

            return;
        }

        #endregion

        #region IReportItem Members
        public PropertyReportItem GetPRI()
        {
            return this;
        }
        #endregion
    }
    #region ColorValues
    internal class ColorConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;   // allow user to also edit the color directly
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(StaticLists.ColorList);
        }
    }
    #endregion

    #region IReportItem
    internal interface IReportItem
    {
        PropertyReportItem GetPRI();
    }
    #endregion
}
