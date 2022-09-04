﻿using AutoFixture;
using AutoMapper;
using BL.Services;
using Castle.Core.Resource;
using DataAccess.Repositories;
using Entities.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using proj.Controllers;
using proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UnitTests
{
    public class UserControllerTests
    {
        [Test]
        public async Task GetAllAsync_200StatusPositive_CallUserService()
        {
            // Arrange
            var service = new Mock<IUserService>();
            var mapper = new Mock<IMapper>();
            var userController = new UserController(service.Object, mapper.Object);
            service.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var data = await userController.GetAllAsync();
            var okObject = data as OkObjectResult;

            //Assert
            service.Verify(x => x.GetAllAsync(), Times.Once());
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task GetById_200StatusPositive_CallUserService()
        {
            // Arrange
            var service = new Mock<IUserService>();

            service.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(
                new User
                {
                    FirstName = "fName",
                    LastName = "lName",
                });
            var mapper = new Mock<IMapper>();
            var userController = new UserController(service.Object, mapper.Object);

            // Act
            var result = await userController.GetById(1);
            var okObject = result as OkObjectResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObject.Value.Should().BeEquivalentTo(new UserModel
            {
                FirstName = "fName",
                LastName = "lName",
            });
        }

        [Test]
        public async Task Create_200StatusPositive_CallUserService()
        {
            // Arrange
            var service = new Mock<IUserService>();
            var mapper = new Mock<IMapper>();
            var testUser = new Fixture(); 
            var userController = new UserController(service.Object, mapper.Object);

            // Act
            var result = await userController.Create(testUser.Create<UserModel>());
            //повертає http status code result
            var okObject = result as StatusCodeResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test]
        public async Task Update_200StatusPositive_CallUserService()
        {
            // Arrange
            var service = new Mock<IUserService>();
            var userForUpdate = new UserModel()
            {
                FirstName = "FirstName",
                LastName = "LastName"
            };
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = fix.Create<User>();
            var mapper = new Mock<IMapper>();
            var userController = new UserController(service.Object, mapper.Object);
            service.Setup(x => x.TryUpdateAsync(user, 1));
            // Act
            await userController.UpdateAsync(userForUpdate, 1);

            //Assert
            service.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            var result = await userController.GetById(1);
            var okObject = result as OkObjectResult;
            okObject.Value.Should().BeEquivalentTo(userForUpdate);
        }

        [Test]
        public async Task Delete_True_CallUserService()
        {
            // Arrange
            var service = new Mock<IUserService>();
            var fix = new Fixture();
            fix.Behaviors.Remove(new ThrowingRecursionBehavior());
            fix.Behaviors.Add(new OmitOnRecursionBehavior());
            var user = fix.Create<User>();
            var mapper = new Mock<IMapper>();
            var userController = new UserController(service.Object, mapper.Object);
            service.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            await userController.DeleteAsync(1);

            //Assert
            service.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once());
        }
    }
}
