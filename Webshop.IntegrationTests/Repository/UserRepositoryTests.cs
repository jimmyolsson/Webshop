using System.Threading.Tasks;
using Webshop.Infrastructure.Data;
using Webshop.Infrastructure.Security.Identity.Entities;
using Webshop.IntegrationTests.TestDBConnection;
using Xunit;

namespace Webshop.IntegrationTests.Repository
{
    public class UserRepositoryTests
    {

        private readonly UserRepository<ApplicationIdentityUser<int>, 
                                        int, 
                                        ApplicationIdentityUserRole<int>, 
                                        ApplicationIdentityRoleClaim<int>,
                                        ApplicationIdentityUserClaim<int>, 
                                        ApplicationIdentityUserLogin<int>,
                                        ApplicationIdentityRole<int>> _userRepository;

        public UserRepositoryTests()
        {
            var dataConnection = TestDBConnectionProvider.GetDataConnection();

            var roleRepository = new RoleRepository<ApplicationIdentityRole<int>, 
                                                    int, 
                                                    ApplicationIdentityUserRole<int>, 
                                                    ApplicationIdentityRoleClaim<int>>(dataConnection);

            _userRepository = new UserRepository<ApplicationIdentityUser<int>,
                                                int,
                                                ApplicationIdentityUserRole<int>,
                                                ApplicationIdentityRoleClaim<int>,
                                                ApplicationIdentityUserClaim<int>,
                                                ApplicationIdentityUserLogin<int>,
                                                ApplicationIdentityRole<int>>(roleRepository, dataConnection);
        }

        [Fact]
        public async Task InsertAsync_AddUser_GivenValidUser()
        {
            var user = new ApplicationIdentityUser<int>()
            {
                Id = 3,
                Username = "Test3",
                Email = "Test3@Test.com",
                Password = "SuperSecret",
                LockoutEnabled = false,
                LockoutEnd = null,
                AccessFailedCount = 0
            };
            

            var id = await _userRepository.InsertAsync(user, new System.Threading.CancellationToken());

            Assert.Equal(user.Id, id);
        }

        [Fact]
        public async Task GetByUserNameAsync_ReturnSingleUser_GivenValidUserName()
        {
            var userNameToGet = "Test";

            var user = await _userRepository.GetByUserNameAsync(userNameToGet, new System.Threading.CancellationToken());

            Assert.Equal(userNameToGet, user.Username);
        }
    }
}
