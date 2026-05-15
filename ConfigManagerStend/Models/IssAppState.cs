namespace ConfigManagerStend.Models
{
    public record IssAppState(string webPoolStatusColor, string srvPoolStatusColor, string webSiteStatusColor, string srvSiteStatusColor)
    {
        public string WebPoolStatusColor = webPoolStatusColor;
        public string SrvPoolStatusColor = srvPoolStatusColor;
        public string WebSiteStatusColor = webSiteStatusColor;
        public string SrvSiteStatusColor = srvSiteStatusColor;
    }
}
