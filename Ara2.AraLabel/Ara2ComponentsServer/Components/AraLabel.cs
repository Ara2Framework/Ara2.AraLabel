// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent]
    public class AraLabel : AraComponentVisualAnchorConteiner, IAraDev
    {
        public AraLabel(IAraObject ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "Div"), ConteinerFather, "AraLabel")
        {
            Buttons = new AraLabelButton(this);

            Click = new AraComponentEvent<EventHandler>(this, "Click");

            this.EventInternal += AraLabel_EventInternal;
            
            this.MinWidth = 10;
            this.MinHeight = 17;
            this.Width = 100;
            this.Height = 17;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraLabel/AraLabel.js");
        }

        private void AraLabel_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                break;
                case "CLICKBUTTON":
                    Buttons.RunEventClick(Convert.ToInt32(Tick.GetTick().Page.Request["codbutton"]));
                break;
            }
        }

        public AraComponentEvent<EventHandler> Click;
        public AraEvent<EventHandler> ClickLink = new AraEvent<EventHandler>();
        
        // Eventos Fim

        public void RemoveInterface()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.RemoveInterface(); \n");
        }

        private string _Text="";
        [AraDevProperty("")]
        [PropertySupportLayout]
        public string Text
        {
            set
            {
                if (this.Childs.Where(a=>!(a is AraDraggable || a is AraResizable || a is AraAnchor)).Count() > 0)
                    return;

                _Text = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetValue('" + AraTools.StringToStringJS(_Text) +  "'); \n");

                if (_Text == "")
                {
                    Buttons = new AraLabelButton(this);
                    if (EventResetButtons.InvokeEvent!=null)
                        EventResetButtons.InvokeEvent();
                }

            }
            get { return _Text; }
        }

        [AraDevEvent]
        public AraEvent<Action> EventResetButtons = new AraEvent<Action>();

        public AraLabelButton Buttons;

        private bool _Visible = true;
        [AraDevProperty(true)]
        [PropertySupportLayout]
        public bool Visible
        {
            set
            {
                _Visible = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetVisible(" + (_Visible == true ? "true" : "false") + "); \n");
            }
            get { return _Visible; }
        }

        public void TextAdd(String vNewValue)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.TextAdd(\"" + AraTools.StringToStringJS(vNewValue) + "\"); \n");
            //foreach (string vTmp in SplitByLength(vNewValue,1))
            //{
            //    Tick.GetTick().Script.Send(" vObj.TextAdd('" + System.Web.HttpUtility.JavaScriptStringEncode(vTmp) + "'); \n");
            //}
        }

        private IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }

        public void TextAddEnd()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.TextAddEnd(); \n");
        }

        public enum ESyleOverflow
        {
            none,
            visible,
            hidden,
            scroll,
            auto,
            inherit
        }

        private ESyleOverflow _SyleOverflow = ESyleOverflow.none;
        [AraDevProperty(ESyleOverflow.none)]
        [PropertySupportLayout]
        public ESyleOverflow SyleOverflow
        {
            get { return _SyleOverflow; }
            set { 
                _SyleOverflow = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetOverflow(\"" + _SyleOverflow.ToString() + "\"); \n");
            }
        }

        public void SetCss(string vName, string vValue)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.SetCss(\"" + AraTools.StringToStringJS(vName) + "\",\"" + AraTools.StringToStringJS(vValue) + "\"); \n");
        }


        public enum ETextAlign
        {
            left,
            right,
            center,
            justify,
            inherit
        }

        private ETextAlign _TextAlign = ETextAlign.left;
        [AraDevProperty(ETextAlign.left)]
        [PropertySupportLayout]
        public ETextAlign TextAlign
        {
            get { return _TextAlign; }
            set
            {
                _TextAlign = value;

                SetCss("text-align", Enum.GetName(_TextAlign.GetType(), _TextAlign));
            }
        }

        public enum ETextVAlign
        {
            none,
            length,
            baseline,
            sub,
            super,
            top,
            text_top,
            middle,
            bottom,
            text_bottom,
            inherit,
        }

        private ETextVAlign _TextVAlign = ETextVAlign.none;
        [AraDevProperty(ETextVAlign.none)]
        [PropertySupportLayout]
        public ETextVAlign TextVAlign
        {
            get { return _TextVAlign; }
            set
            {
                _TextVAlign = value;
                if (_TextVAlign!= ETextVAlign.none)
                    _TextVAlignValue = null;

                if (_TextVAlign == ETextVAlign.none)
                    SetCss("vertical-align", "");
                else
                    SetCss("vertical-align", Enum.GetName(_TextVAlign.GetType(), _TextVAlign).Replace("_","-"));
            }
        }

        private AraDistance _TextVAlignValue = null;
        [AraDevProperty]
        [PropertySupportLayout]
        public AraDistance TextVAlignValue
        {
            get { return _TextVAlignValue; }
            set
            {
                _TextVAlignValue = value;
                if (_TextVAlignValue!=null)
                    _TextVAlign = ETextVAlign.none;

                if (_TextVAlignValue == null)
                    SetCss("vertical-align", "");
                else
                    SetCss("vertical-align", "\"" + _TextVAlignValue.ToString() + "\"");
            }
        }

        public void Print()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send("$('<div>' + vObj.Obj.innerHTML + '</div>').printElement(); \n");
        }

        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }
        #endregion

    }

}
