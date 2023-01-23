using System;
using Firebase.Auth.Providers;
using Firebase.Auth;
using HB.SharedObject;
using Microsoft.Extensions.Configuration;
using Firebase.Storage;
using System.IO;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using System.Text;
using JasperFx.Core;
using HB.Service.Enum;

namespace HB.Service.Firebase
{
	public class FirebaseService : IFirebaseService
	{
        private readonly FirebaseAuthConfig _config;
        private readonly FirebaseAuthClient _client;
        private readonly IConfiguration _configuration;

        public FirebaseService(FirebaseAuthConfig config , IConfiguration configuration)
        {
            this._config = config;
            this._client = new FirebaseAuthClient(_config);
            this._configuration = configuration;
        }

        public async Task<ReturnState<object>> AddUser(string email, string password, string displayName)
        {
            var result = await _client.CreateUserWithEmailAndPasswordAsync(email, password , displayName);
            return new ReturnState<object>(true);
        }

        public async Task<ReturnState<object>> DeleteUser(string email, string password)
        {
            var userResult = await GetUserInformation(email, password);

            await _client.User.DeleteAsync();

            return new ReturnState<object>(true);
        }

        public async Task<ReturnState<object>> GetUser(string email, string password)
        {
            var userResult = await GetUserInformation(email , password);

            return new ReturnState<object>(userResult);
        }

        public async Task<ReturnState<object>> GetUserImges(List<string> fileList , string email , string password)
        {
            var userResult = await GetUserInformation(email, password);

            var cancellation = new CancellationTokenSource();

            var fileListUrl = new List<string>();

            foreach (var item in fileList)
            {
                var task = await new FirebaseStorage(
               _configuration.GetValue<string>("Firebase:Bucket"),
               new FirebaseStorageOptions
               {
                   AuthTokenAsyncFactory = () => Task.FromResult(userResult.User.Credential.IdToken),
                   ThrowOnCancel = true
               })
               .Child("UsersImg")
               .Child($"{userResult.User.Info.Uid}")
               .Child(item)
               .GetDownloadUrlAsync();

               fileList.Add(task);
            }

            return new ReturnState<object>(fileListUrl);
        }

        public async Task<ReturnState<object>> UpdateUser(string email, string password)
        {
            var userResult = await GetUserInformation(email, password);
            //MARK: Refactor this code. This method accept UpdateUser model!

            throw new NotImplementedException();
        }

        public async Task<ReturnState<object>> UploadImage(IFormFile formFile , string email , string password)
        {
            var userResult = await GetUserInformation(email, password);

            var cancellation = new CancellationTokenSource();

            MemoryStream stream = new MemoryStream(
                formFile.OpenReadStream()
                .ReadAllBytes());

            var task = new FirebaseStorage(
                _configuration.GetValue<string>("Firebase:Bucket"),
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(userResult.User.Credential.IdToken),
                    ThrowOnCancel = true
                })
                .Child("UsersImg")
                .Child($"{userResult.User.Info.Uid}")
                .Child($"{formFile.FileName}")
                .PutAsync(stream, cancellation.Token);

            return new ReturnState<object>(task);
        }

        private async Task<UserCredential?> GetUserInformation(string email , string password)
        {
            var userResult = await _client.SignInWithEmailAndPasswordAsync(email, password)
            ?? throw new Exception("User not found!");

            return userResult;
        }
    }
}

