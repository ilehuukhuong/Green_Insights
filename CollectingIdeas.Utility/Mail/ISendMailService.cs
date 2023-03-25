using System.Threading.Tasks;
using CollectingIdeas.Utility.Mail;


public interface ISendMailService
{
    Task SendMail(MailContent mailContent);

    Task IdeaSubmissionEmail(string email, string fullname, string title);
}