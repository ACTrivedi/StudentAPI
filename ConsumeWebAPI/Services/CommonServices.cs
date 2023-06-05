using ConsumeWebAPI.Enums;

namespace ConsumeWebAPI.Services
{
    public class CommonServices
    {
        public static string ShowAlert(Alerts obj, string message)
        {
            string alertDiv = null;
            switch (obj)
            {
                case Alerts.Success:             
                    alertDiv = "<div class='alert alert-success alert-dismissible fade show mt-5' role='alert'> " + message + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<div class='alert alert-danger alert-dismissible fade show mt-5' role='alert'> " + message + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>";
                    break;
                case Alerts.Info:
                    alertDiv = "<div class='alert alert-info alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Info!</ strong > " + message + "</a>.</div>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<div class='alert alert-warning alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Warning!</strong> " + message + "</a>.</div>";
                    break;
            }
            return alertDiv;
        }
    }
}
