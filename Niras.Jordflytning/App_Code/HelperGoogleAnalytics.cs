using System.Configuration;
using System.Text;


namespace System.Web.Mvc
{
  public static partial class HtmlHelpers    
  {

    public static HtmlString Analytics(this HtmlHelper htmlHelper, string urchin, string domainName)
    {
      StringBuilder sb = new StringBuilder();
      /*
<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-46104055-1', 'flytjord.dk');
  ga('send', 'pageview');

</script>       
       */


      sb.AppendLine("<script type='text/javascript'>");
      sb.AppendLine("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){");
      sb.AppendLine("(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),");
      sb.AppendLine("m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)");
      sb.AppendLine("})(window,document,'script','//www.google-analytics.com/analytics.js','ga');");
      sb.AppendLine("ga('create', '" + urchin + "', '" + domainName + "');");
      sb.AppendLine("ga('send', 'pageview');");
      sb.AppendLine("</script>");

      return new HtmlString(sb.ToString());
    }

    /// <summary>
    /// Pull the urchin and domain name from Web.Config
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <returns></returns>
    public static HtmlString Analytics(this HtmlHelper htmlHelper)
    {
      //pull values from Config
      string urchin = ConfigurationManager.AppSettings["ga-urchin"];
      string domainName = ConfigurationManager.AppSettings["ga-domainName"];
      /* TOK: Fjernet 26.10.2018 efter aftale med Bo.   return Analytics(htmlHelper, urchin, domainName);   */
      return new HtmlString("");
    }
  }
}