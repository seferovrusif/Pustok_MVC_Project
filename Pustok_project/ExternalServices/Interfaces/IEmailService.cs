namespace Pustok_project.ExternalServices.Interfaces
{
    public interface IEmailService
    {
        void Send(string toMail,string subject, string body,bool isHtml=true);
    }
}
