using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PZ.Controllers.Api.DTO
{
    [Serializable]
    [KnownType(typeof(MenuBundleDTO))]
    [DataContract(Namespace = "", Name = "items")]
    public class MenuBundleDTO
    {
        [DataMember]
        public MenuDTO[] menus;
    }

    [Serializable]
    [KnownType(typeof(MenuDTO))]
    [DataContract(Namespace = "", Name="item")]
    public class MenuDTO
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string category { get; set; }
        [DataMember]
        public string subcategory { get; set; }
    }
}