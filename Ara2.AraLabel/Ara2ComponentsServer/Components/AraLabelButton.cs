// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ara2.Components
{
    [Serializable]
    public class AraLabelButton
    {
        AraObjectInstance<AraLabel> _Label = new AraObjectInstance<AraLabel>();
        private AraLabel Label
        {
            get { return _Label.Object; }
            set { _Label.Object = value; }
        }

        public AraLabelButton(AraLabel vLabel)
        {
            Label = vLabel;
        }



        private List<CAraLabelButton> AraLabelButtonReturn = new List<CAraLabelButton>();
        private int AraLabelButtonReturn_New = 1;
        [Serializable]
        private class CAraLabelButton
        {
            public int Key;
            public AraEvent<Delegate> Event;
            public object[] Parametros;
            public DateTime Date;

            public void InvokeEvent()
            {
                Event.InvokeEvent.DynamicInvoke(Parametros);
            }
        }

        public enum SStyleButton
        {
            Button = 1,
            Link = 2
        }

        public delegate void Click_delegate(AraLabel Object, System.Collections.Hashtable Parans);
        public string Add(Hashtable Parans, Click_delegate Click)
        {
            return Add("", ButtonIco.None, Parans, Click);
        }

        public string Add(ButtonIco Ico, Hashtable Parans, Click_delegate Click)
        {
            return Add("", Ico, Parans, Click);
        }

        public string Add(string vCaption, Hashtable Parans, Click_delegate Click)
        {
            return Add(vCaption, ButtonIco.None, Parans, Click);
        }

        public string Add(string vCaption, Hashtable Parans, Click_delegate Click, SStyleButton Style)
        {
            return Add(vCaption, ButtonIco.None, Parans, Click, Style);
        }

        public string Add(string vCaption, ButtonIco Ico, Hashtable Parans, Click_delegate Click)
        {
            return Add(vCaption, Ico, Parans, Click, SStyleButton.Button);
        }

        public string Add(string vCaption, ButtonIco Ico, Hashtable Parans, Click_delegate Click, SStyleButton Style)
        {
            return Add("#", vCaption, Ico, Parans, Click, Style);
        }

        public string Add(string vHref, string vCaption, ButtonIco Ico, Hashtable Parans, Click_delegate Click, SStyleButton Style)
        {
            return Add<Click_delegate>(vHref, vCaption, Ico, Style, Click, Label, Parans);
        }

        public string Add<T>(string vCaption, T vEvent, params object[] vParametros)
        {
            return Add<T>(vCaption, ButtonIco.None, vEvent, vParametros);
        }

        public string Add<T>(string vCaption, ButtonIco Ico, T vEvent, params object[] vParametros)
        {
            return Add<T>(vCaption, Ico, SStyleButton.Button, vEvent, vParametros);
        }

        public string Add<T>(ButtonIco Ico, T vEvent, params object[] vParametros)
        {
            return Add<T>("", Ico, SStyleButton.Button, vEvent, vParametros);
        }

        public string Add<T>(string vCaption, ButtonIco Ico, SStyleButton Style, T vEvent, params object[] vParametros)
        {
            return Add<T>("#", vCaption, Ico, Style, vEvent, vParametros);
        }

        public string Add<T>(ButtonIco Ico, SStyleButton Style, T vEvent, params object[] vParametros)
        {
            return Add<T>("#", "", Ico, Style, vEvent, vParametros);
        }

        public string Add<T>(string vHref, string vCaption, ButtonIco Ico, SStyleButton Style, T vEvent, params object[] vParametros)
        {
            Delegate vRetorno = (Delegate)(object)vEvent;

            int vCodB = AraLabelButtonReturn_New;
            AraLabelButtonReturn_New++;

            AraEvent<Delegate> Event = new AraEvent<Delegate>();
            Event += vRetorno;

            AraLabelButtonReturn.Add(new CAraLabelButton()
            {
                Key = vCodB,
                Event = Event,
                Parametros = vParametros,
                Date = DateTime.Now
            });


            string vIdB = "AraLabel_Button_" + Label.InstanceID + "_" + vCodB;

            string ScriptIco = "";
            if (Ico != ButtonIco.None)
                ScriptIco += "<span class='ui-icon ui-icon-" + Ico.ToString().Replace("_", "-") + " tree-plus '></span>";

            string vReturn = "";
            if (Style == SStyleButton.Button)
            {
                vReturn += "<button id='" + vIdB + "' ";
                vReturn += " onclick=\"javascript:Ara.GetObject('" + Label.InstanceID + "').OnClickButton('" + vCodB + "');return false;\"";
                vReturn += " class='ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only' ";
                vReturn += ">";
                vReturn += "<span>";
                vReturn += "<table>";
                vReturn += "<tr>";
                if (ScriptIco != "")
                {
                    vReturn += "<td>";
                    vReturn += "<div class='tree-wrap tree-wrap-ltr' style='width: 18px;'>";
                    vReturn += ScriptIco;
                    vReturn += "</div>";
                    vReturn += "</td>";
                }
                if (vCaption != "")
                {
                    vReturn += "<td>";
                    vReturn += vCaption;
                    vReturn += "</td>";
                }
                vReturn += "</tr>";
                vReturn += "</table>";
                vReturn += "</span>";
                vReturn += "</button>";
            }
            else if (Style == SStyleButton.Link)
            {
                vReturn += "<a href='" + vHref + "' onclick=\"javascript:Ara.GetObject('" + Label.InstanceID + "').OnClickButton('" + vCodB + "');return false;\">";
                vReturn += ScriptIco;
                vReturn += vCaption;
                vReturn += "</a>";
            }

            return vReturn;
        }



        public void RunEventClick(int vCod)
        {
            try
            {
                AraLabelButtonReturn
                    .Where(a => a.Key == vCod)
                    .First()
                    .InvokeEvent();
            }
            catch (Exception err)
            {
                throw new Exception("Error on click button label.\n" + err.Message);
            }
        }


        public enum ButtonIco
        {
            None,
            carat_1_n,
            carat_1_ne,
            carat_1_e,
            carat_1_se,
            carat_1_s,
            carat_1_sw,
            carat_1_w,
            carat_1_nw,
            carat_2_n_s,
            carat_2_e_w,
            triangle_1_n,
            triangle_1_ne,
            triangle_1_e,
            triangle_1_se,
            triangle_1_s,
            triangle_1_sw,
            triangle_1_w,
            triangle_1_nw,
            triangle_2_n_s,
            triangle_2_e_w,
            arrow_1_n,
            arrow_1_ne,
            arrow_1_e,
            arrow_1_se,
            arrow_1_s,
            arrow_1_sw,
            arrow_1_w,
            arrow_1_nw,
            arrow_2_n_s,
            arrow_2_ne_sw,
            arrow_2_e_w,
            arrow_2_se_nw,
            arrowstop_1_n,
            arrowstop_1_e,
            arrowstop_1_s,
            arrowstop_1_w,
            arrowthick_1_n,
            arrowthick_1_ne,
            arrowthick_1_e,
            arrowthick_1_se,
            arrowthick_1_s,
            arrowthick_1_sw,
            arrowthick_1_w,
            arrowthick_1_nw,
            arrowthick_2_n_s,
            arrowthick_2_ne_sw,
            arrowthick_2_e_w,
            arrowthick_2_se_nw,
            arrowthickstop_1_n,
            arrowthickstop_1_e,
            arrowthickstop_1_s,
            arrowthickstop_1_w,
            arrowreturnthick_1_w,
            arrowreturnthick_1_n,
            arrowreturnthick_1_e,
            arrowreturnthick_1_s,
            arrowreturn_1_w,
            arrowreturn_1_n,
            arrowreturn_1_e,
            arrowreturn_1_s,
            arrowrefresh_1_w,
            arrowrefresh_1_n,
            arrowrefresh_1_e,
            arrowrefresh_1_s,
            arrow_4,
            arrow_4_diag,
            extlink,
            newwin,
            refresh,
            shuffle,
            transfer_e_w,
            transferthick_e_w,
            folder_collapsed,
            folder_open,
            document,
            document_b,
            note,
            mail_closed,
            mail_open,
            suitcase,
            comment,
            person,
            print,
            trash,
            locked,
            unlocked,
            bookmark,
            tag,
            home,
            flag,
            calendar,
            cart,
            pencil,
            clock,
            disk,
            calculator,
            zoomin,
            zoomout,
            search,
            wrench,
            gear,
            heart,
            star,
            link,
            cancel,
            plus,
            plusthick,
            minus,
            minusthick,
            close,
            closethick,
            key,
            lightbulb,
            scissors,
            clipboard,
            copy,
            contact,
            image,
            video,
            script,
            alert,
            info,
            notice,
            help,
            check,
            bullet,
            radio_off,
            radio_on,
            pin_w,
            pin_s,
            play,
            pause,
            seek_next,
            seek_prev,
            seek_end,
            seek_start,
            seek_first,
            stop,
            eject,
            volume_off,
            volume_on,
            power,
            signal_diag,
            signal,
            battery_0,
            battery_1,
            battery_2,
            battery_3,
            circle_plus,
            circle_minus,
            circle_close,
            circle_triangle_e,
            circle_triangle_s,
            circle_triangle_w,
            circle_triangle_n,
            circle_arrow_e,
            circle_arrow_s,
            circle_arrow_w,
            circle_arrow_n,
            circle_zoomin,
            circle_zoomout,
            circle_check,
            circlesmall_plus,
            circlesmall_minus,
            circlesmall_close,
            squaresmall_plus,
            squaresmall_minus,
            squaresmall_close,
            grip_dotted_vertical,
            grip_dotted_horizontal,
            grip_solid_vertical,
            grip_solid_horizontal,
            gripsmall_diagonal_se,
            grip_diagonal_se
        }
    }

}
