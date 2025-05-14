﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;

namespace UsersMS.Application.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeycloakService _keycloakService;

        public DeleteUserCommandHandler(IUserRepository userRepository, IKeycloakService keycloakService)
        {
            _userRepository = userRepository;
            _keycloakService = keycloakService;
        }

        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.DeleteUserDto.UserId;

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ApplicationException($"No user found with ID {userId}");

            if (string.IsNullOrEmpty(user.Email))
                throw new ApplicationException("User email cannot be null or empty.");

            var token = await _keycloakService.GetAdminTokenAsync();
            await _keycloakService.DisableUserAsync(user.Email, token);
            await _userRepository.DeleteAsync(userId);

            return "User successfully disabled.";
        }
    }
}

