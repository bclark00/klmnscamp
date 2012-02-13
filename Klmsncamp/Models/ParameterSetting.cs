using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Klmsncamp.Models
{
    public class ParameterSetting
    {
        public int ParameterSettingID { get; set; }

        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        [Display(Name = "Parametre Adı")]
        public string Description { get; set; }

        [Display(Name = "Parametre Değeri")]
        [MaxLength(100, ErrorMessage = "100 karakterden uzun olamaz")]
        [AllowHtml]
        public string ParameterValue { get; set; }

        public virtual string BannerFileName
        {
            get
            {
                using (KlmsnContext db = new KlmsnContext())
                {
                    string bfile;
                    try
                    {
                        bfile = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 12).SingleOrDefault().ParameterValue;
                    }
                    catch
                    {
                        bfile = "parameter-notfound.png";
                    }

                    db.Dispose();
                    return bfile;
                }
            }
        }

        public virtual string BannerOverlayString
        {
            get
            {
                using (KlmsnContext db = new KlmsnContext())
                {
                    string overlay_;
                    try
                    {
                        overlay_ = db.ParameterSettings.AsNoTracking().Where(i => i.ParameterSettingID == 13).SingleOrDefault().ParameterValue;
                    }
                    catch
                    {
                        overlay_ = "Parameter N/A";
                    }

                    db.Dispose();
                    return overlay_;
                }
            }
        }
    }
}