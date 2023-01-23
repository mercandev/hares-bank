using System;
using HB.SharedObject;
using Microsoft.AspNetCore.Http;

namespace HB.Service.Firebase
{
	public interface IFirebaseService
	{
		Task<ReturnState<object>> AddUser(string email , string password, string displayName);
		Task<ReturnState<object>> UpdateUser(string email, string password);
        Task<ReturnState<object>> DeleteUser(string email, string password);
		Task<ReturnState<object>> GetUser(string email , string password);
		Task<ReturnState<object>> UploadImage(IFormFile formFile, string email, string password);
		Task<ReturnState<object>> GetUserImges(List<string> fileList, string email, string password);
    }
}

