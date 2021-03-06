using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProjectTwo.Models
{
    public class WoWClass
    {
        [Key]
        public int ClassID { get; set; }

        public string ClassName { get; set; }

        public string ClassSpec { get; set; }

        public bool ClassPic { get; set; }

        public string PicExtension { get; set; }

        //a class can play for multiple comps
        [ForeignKey("WoWComp")]
        public int? CompID { get; set;}
        public virtual WoWComp WoWComp { get; set; }


    }
    public class WoWClassDto
    {
        [Key]
        public int ClassID { get; set; }

        [DisplayName("Class Name")]
        public string ClassName { get; set; }

        [DisplayName("Class Specialization")]
        public string ClassSpec { get; set; }

        public bool ClassPic { get; set; }

        public string PicExtension { get; set; }

    }

}