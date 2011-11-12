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
using System.Collections;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Drawing.Design;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyExpr - 
    /// </summary>
    [TypeConverter(typeof(PropertyExprConverter)),
        Editor(typeof(PropertyExprUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyExpr
    {
        string _expr;
        //ArrayList _list;

        internal PropertyExpr(string ex)
        {
            _expr = ex;
        }
        //internal PropertyExpr(string ex, ArrayList list)
        //{
        //    _expr = ex;
        //    _list = list;
        //}
        internal string Expression
        {
            get { return _expr; }
        }
        
        public override string ToString()
        {
            return _expr;
        }
    }

    internal class PropertyExprConverter : StringConverter
    {

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyExpr))
                return true;
            
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {

            if (t == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyExpr)
            {
                PropertyExpr pe = value as PropertyExpr;
                return pe.Expression;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context,
                                      CultureInfo culture, object value)
        {
            if (!(value is string))
                return base.ConvertFrom(context, culture, value);

            return new PropertyExpr(value as string);
        }
    }

    internal class PropertyExprUIEditor : UITypeEditor
    {
        internal PropertyExprUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
                                        IServiceProvider provider,
                                        object value)
        {

            if ((context == null) || (provider == null))
                return base.EditValue(context, provider, value);

            // Access the Property Browser's UI display service
            IWindowsFormsEditorService editorService =
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService == null)
                return base.EditValue(context, provider, value);

            // Create an instance of the UI editor form
            IReportItem iri = context.Instance as IReportItem;
            if (iri == null)
                return base.EditValue(context, provider, value);
            PropertyReportItem pri = iri.GetPRI();

            PropertyExpr pe = value as PropertyExpr;
            if (pe == null)
                return base.EditValue(context, provider, value);

            using (DialogExprEditor de = new DialogExprEditor(pri.Draw, pe.Expression, pri.Node))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(de) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyExpr(de.Expression);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }


}