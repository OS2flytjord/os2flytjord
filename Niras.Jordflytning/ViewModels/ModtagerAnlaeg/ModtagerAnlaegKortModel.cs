using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.ModtagerAnlaeg
{
  public class ModtagerAnlaegKortModel
  {
    //Kort
    public string KortApiUrl { get; set; }
    public string KortPageModtagere { get; set; }
    public string KortSite { get; set; }

    public string GetMapBoundLowerX(string wkt)
    {
      double? d = double.MaxValue;
      try
      {
        var g = DbGeometry.FromText(wkt, 25832);
        if (g.PointCount > 1)
        {
          for (int i = 1; i <= g.Boundary.PointCount; i++)
          {
            var ordiant = g.PointAt(i).XCoordinate;
            if (ordiant < d)
              d = ordiant;
          }
        }
        else
        {
          d = g.XCoordinate - 500;
        }

      }
      catch
      { }
      if (d == double.MaxValue)
        return "";

      return Math.Floor(d.GetValueOrDefault()).ToString();
    }
    public string GetMapBoundLowerY(string wkt)
    {
      double? d = double.MaxValue;
      try
      {
        var g = DbGeometry.FromText(wkt, 25832);
        if (g.PointCount > 1)
        {
          for (int i = 1; i <= g.Boundary.PointCount; i++)
          {
            var ordiant = g.PointAt(i).YCoordinate;
            if (ordiant < d)
              d = ordiant;
          }
        }
        else
        {
          d = g.YCoordinate - 500;
        }
      }
      catch
      { }
      if (d == double.MaxValue)
        return "";

      return Math.Floor(d.GetValueOrDefault()).ToString();
    }
    public string GetMapBoundUpperX(string wkt)
    {
      double? d = double.MinValue;
      try
      {
        var g = DbGeometry.FromText(wkt, 25832);
        if (g.PointCount > 1)
        {
          for (int i = 1; i <= g.Boundary.PointCount; i++)
          {
            var ordiant = g.PointAt(i).XCoordinate;
            if (ordiant > d)
              d = ordiant;
          }
        }
        else
        {
          d = g.XCoordinate + 500;
        }
      }
      catch
      { }
      if (d == double.MinValue)
        return "";

      return Math.Floor(d.GetValueOrDefault()).ToString();
    }
    public string GetMapBoundUpperY(string wkt)
    {
      double? d = double.MinValue;
      try
      {
        var g = DbGeometry.FromText(wkt, 25832);
        if (g.PointCount > 1)
        {
          for (int i = 1; i <= g.Boundary.PointCount; i++)
          {
            var ordiant = g.PointAt(i).YCoordinate;
            if (ordiant > d)
              d = ordiant;
          }
        }
        else
        {
          d = g.YCoordinate + 500;
        }
      }
      catch
      { }
      if (d == double.MinValue)
        return "";

      return Math.Floor(d.GetValueOrDefault()).ToString();
    }

    public string MapModtagerAnlaegLx { get; set; }
    public string MapModtagerAnlaegLy { get; set; }
    public string MapModtagerAnlaegUx { get; set; }
    public string MapModtagerAnlaegUy { get; set; }

    public string wkt { get; set; }

    public bool ShowDigitaliserButton { get; set; }
  }
}