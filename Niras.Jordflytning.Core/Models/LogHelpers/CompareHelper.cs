using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.LogHelpers
{
  public static class CompareHelper
  {
    //public static List<Delta> DetailedCompare<T>(this T val1, T val2)
    //{
    //  List<Delta> Deltas = new List<Delta>();
    //  FieldInfo[] fi = val1.GetType().GetFields();
    //  foreach (FieldInfo f in fi)
    //  {
    //    Delta v = new Delta();
    //    v.Navn = f.Name;
    //    v.Foer = f.GetValue(val1);
    //    v.Efter = f.GetValue(val2);
    //    if (!v.Foer.Equals(v.Efter))
    //      Deltas.Add(v);

    //  }

    //  var pi = val1.GetType().GetProperties();
    //  foreach (var p in pi)
    //  {
    //    if (!p.Name.ToLower().Contains("id"))
    //    {

    //      Delta v = new Delta();
    //    v.Navn   = p.Name;
    //    v.Foer = p.GetValue(val1);
    //    v.Efter = p.GetValue(val2);
    //      if (v.Foer != null && v.Efter != null)
    //      {
    //        if (!v.Foer.Equals(v.Efter))
    //          Deltas.Add(v);
    //      }
    //      else if (v.Foer == null && v.Efter != null)
    //      {
    //        Deltas.Add(v);
    //      }
    //      else if (v.Efter == null && v.Foer != null)
    //      {
    //        Deltas.Add(v);
    //      }


    //    }

    //  }
    //  return Deltas;
    //}

    public static Delta StringCompare(string propname, string val1, string val2)
    {
      if (val1 != val2)
      {
        var v = new Delta();
        v.Navn = propname;
        v.Foer = val1;
        v.Efter = val2;
        return v;
      }
      return null;
    }
  }
}
