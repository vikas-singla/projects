using System.ComponentModel.DataAnnotations;
using Oceanus.Data;

namespace Oceanus.ViewModels
{
    public class SignUpViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        private OceanusEntities _dbContainer;

        public SignUpViewModel()
        {
            _dbContainer = new OceanusEntities();
        }

        public void SignUpEmail(string firstName, string lastName, string emailAddress)
        {
            // add user to DB
            _dbContainer.Registrations.AddObject(new Registration()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = emailAddress
            });

            _dbContainer.SaveChanges();

            // TODO: send email
        }
    }
}