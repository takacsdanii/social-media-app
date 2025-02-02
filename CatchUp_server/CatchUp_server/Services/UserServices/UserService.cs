﻿using CatchUp_server.Db;
using CatchUp_server.Models.UserModels;
using CatchUp_server.Services.UserContentServices;
using CatchUp_server.ViewModels.UserViewModel;
using CatchUp_server.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatchUp_server.Services.UserServices
{
    public class UserService
    {
        private readonly ApiDbContext _context;
        private readonly MediaFoldersService _mediaFoldersService;

        public UserService(ApiDbContext context, MediaFoldersService mediaFoldersService)
        {
            _context = context;
            _mediaFoldersService = mediaFoldersService;
        }

        private UserViewModel MapUserToViewModel(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Bio = user.Bio,
                ProfilePicUrl = user.ProfilePicUrl,
                CoverPicUrl = user.CoverPicUrl,
                RegisteredAt = user.RegisteredAt,
                FollowersCount = _context.FriendShips.Where(f => f.FollowedUserId == user.Id).Count(),
                FollowingCount = _context.FriendShips.Where(f => f.FollowerUserId == user.Id).Count()
            };
        }

        public IReadOnlyCollection<UserViewModel> List()
        {
            var users = _context.Users.ToList();
            var userViewModels = users.Select(user => MapUserToViewModel(user)).ToList();
            return userViewModels;
        }

        public User DeleteUser(string userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                var usersFriendShips = _context.FriendShips
                    .Where(f => f.FollowerUserId == userId || f.FollowedUserId == userId)
                    .ToList();

                var comments = _context.Comments.Where(c => c.UserId == userId).ToList();
                var likes = _context.Likes.Where(l => l.UserId == userId).ToList();

                _context.FriendShips.RemoveRange(usersFriendShips);
                _context.Likes.RemoveRange(likes);
                _context.Comments.RemoveRange(comments);
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            _mediaFoldersService.DeleteUserMediaFolders(userId);

            return user;
        }

        public UserViewModel GetUser(string id)
        {
            var user = _context.Users.SingleOrDefault(u => id == u.Id);
            return user != null ? MapUserToViewModel(user) : null;
        }

        public IReadOnlyCollection<UserViewModel> GetUsers(string name)
        {
            var users = _context.Users
                .Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name) || u.UserName.Contains(name))
                .Select(u => MapUserToViewModel(u))
                .ToList();
            return users;
        }

        public UserViewModel GetUserByEmail(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            return user != null ? MapUserToViewModel(user) : null;
        }

        public IdentityResult UpdateUser(UserViewModel userModel)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userModel.Id);
            if (user != null)
            {
                bool doesUserNameExist = _context.Users.Any(u => u.UserName == userModel.UserName && u.Id != userModel.Id);
                if (doesUserNameExist)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "This username is already in use." });
                }

                bool doesEmailExist = _context.Users.Any(u => u.Email == userModel.Email && u.Id != userModel.Id);
                if (doesEmailExist)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "This email is already in use." });
                }

                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.UserName = userModel.UserName;
                user.NormalizedUserName = userModel.UserName.ToUpper();
                user.Email = userModel.Email;
                user.NormalizedEmail = userModel.Email.ToUpper();
                user.BirthDate = userModel.BirthDate;

                _context.SaveChanges();
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        public Gender? UpdateGender(string userId, Gender gender)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Gender = gender;
                _context.SaveChanges();
                return user.Gender;
            }
            return null;
        }

        public IReadOnlyCollection<SearchUserViewModel> SearchUsers(string searchString)
        {
            var users = _context.Users
                .Where(u => u.UserName.Contains(searchString) ||
                            u.FirstName.Contains(searchString) ||
                            u.LastName.Contains(searchString))
                .Select(u => new SearchUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePicUrl = u.ProfilePicUrl,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToList();

            return users;
        }
    }
}
