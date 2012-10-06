using pcAmerica.DesktopPOS.API.Client.EmployeeService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class EmployeeAPI
    {
        /// <summary>
        /// Returns the employee that's currently logged into the server POS workstation
        /// </summary>
        /// <returns>Information about the employee. NULL if no one is currently logged into the workstation</returns>
        public Employee GetCurrentUser()
        {
            using (var client = new EmployeeServiceClient())
            {
                client.Open();
                return client.GetCurrentUser();
            }
        }

        /// <summary>
        /// Verifies that an employee's credentials are valid.  Use this method to validate the login to your own program.
        /// </summary>
        /// <param name="userName">The employee's username</param>
        /// <param name="password">The employee's password</param>
        /// <returns>Information about the employee. NULL if the authentication fails.</returns>
        public Employee AuthenticateEmployee(string userName, string password)
        {
            using (var client = new EmployeeServiceClient())
            {
                client.Open();
                return client.AuthenticateEmployee(userName, password);
            }
        }
    }
}