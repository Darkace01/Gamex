using Gamex.DTO;

namespace Gamex.Test.UnitTest;

public class PictureServiceTest : TestBase
{
    [Fact]
    public async Task GetPicture_ShouldReturnPicture()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPicture_ShouldReturnPicture));
        var pictureService = MockPictureService(dbContext);
        var pictureToGet = dbContext.Pictures.FirstOrDefault();

        // Act
        var picture = await pictureService.GetPicture(pictureToGet.Id);

        // Assert
        Assert.NotNull(picture);
        Assert.Equal(pictureToGet.Id, picture.Id);
    }

    [Fact]
    public async Task CreatePicture_ShouldCreatePicture()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(CreatePicture_ShouldCreatePicture));
        var pictureService = MockPictureService(dbContext);

        PictureCreateDTO newPicture = new("https://res.cloudinary.com/dx3vxwusq/image/upload/v1629788239/Default%20Images/Default%20Profile%20Picture.png", "Default Profile Picture"
        );

        // Act
        var picture = await pictureService.CreatePicture(newPicture);

        // Assert
        Assert.NotNull(picture);
        Assert.Equal(picture.FileUrl, newPicture.FileUrl);
        // Add more assertions to check the properties of the created picture
    }

    [Fact]
    public async Task UpdatePicture_ShouldUpdatePicture()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(UpdatePicture_ShouldUpdatePicture));
        var pictureService = MockPictureService(dbContext);
        var pictureToUpdate = dbContext.Pictures.FirstOrDefault();

        PictureUpdateDTO updatedPicture = new(new Guid(),"","");

        // Act
        await pictureService.UpdatePicture(updatedPicture);

        // Assert
        var updatedPictureInDb = dbContext.Pictures.FirstOrDefault(p => p.Id == pictureToUpdate.Id);
        Assert.NotNull(updatedPictureInDb);
        // Add more assertions to check the properties of the updated picture
    }

    [Fact]
    public async Task DeletePicture_ShouldDeletePicture()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(DeletePicture_ShouldDeletePicture));
        var pictureService = MockPictureService(dbContext);
        var pictureToDelete = dbContext.Pictures.FirstOrDefault();

        // Act
        await pictureService.DeletePicture(pictureToDelete.Id);

        // Assert
        var deletedPictureInDb = dbContext.Pictures.FirstOrDefault(p => p.Id == pictureToDelete.Id);
        Assert.Null(deletedPictureInDb);
    }

    [Fact]
    public async Task GetPictureByPublicId_ShouldReturnPicture()
    {
        // Arrange
        var dbContext = GetSampleData(nameof(GetPictureByPublicId_ShouldReturnPicture));
        var pictureService = MockPictureService(dbContext);
        var pictureToGet = dbContext.Pictures.FirstOrDefault();

        // Act
        var picture = await pictureService.GetPictureByPublicId(pictureToGet.PublicId);

        // Assert
        Assert.NotNull(picture);
        Assert.Equal(pictureToGet.Id, picture.Id);
    }
    #region Helpers
    private PictureService MockPictureService(GamexDbContext dbContext)
    {
        return new PictureService(dbContext);
    }
    #endregion
}
