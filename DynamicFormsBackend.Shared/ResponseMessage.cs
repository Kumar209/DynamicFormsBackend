using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsBackend.Shared
{
    public class ResponseMessage
    {
        public const string validationFailed = "Validation failed";
        public const string wrongEmail = "Wrong Email";
        public const string wrongPassword = "Wrong Password";
        public const string loginSuccess = "Login successfully";
        public const string wrongCredential = "Wrong Credentials";
        public const string deleteUserSuccess = "User is deleted";
        public const string tokenError = "Your token is expired or invalid token";
        public const string unauthorizeUser = "Unauthorized User";
        public const string getdetailMessage = "Success";
        public const string internalServerError = "Internal server error";

        public const string sourceTemplateCreationSuccess = "Succesfully published template";
        public const string NullTemplateError = "Template or sections cannot be null";

        public const string questionCreationSuccess = "Successfully created question";
        public const string NotFoundQuestion = "Question not found";

        public const string questionDeleted = "Question deleted successfully";

        public const string NotFoundForm = "Form not found";

        public const string formDeleted = "Form deleted successfully";





        public const string somethingWrongError = "Something went wrong";
    }
}
