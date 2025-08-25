using MovieRecommendation.Services.Auth;
using MovieRecommendation.Services.Movie;

namespace MovieRecommendation.Tests.Auth;

public class WatchlistTests
{
    [Fact]
    public async Task CanReadAddAndRemoveToWatchlistTest()
    {
        var dbContext = TestUtils.GetInMemoryDbContext();
        var userService = new DbUserService(dbContext, TestUtils.GetAuthMapper());
        var categoryService = new DbCategoryService(dbContext, TestUtils.GetMovieMapper());
        var movieService = new DbMovieService(dbContext, TestUtils.GetMovieMapper());

        var user = await TestUtils.CreateUser(userService);
        var cat1 = await TestUtils.CreateCategory(categoryService);
        var cat2 = await TestUtils.CreateCategory(categoryService);
        var movie1 = await TestUtils.CreateMovie(movieService, [cat1]);
        var movie2 = await TestUtils.CreateMovie(movieService, [cat2]);

        var watchlist = await userService.GetWatchedMovieIdsAsync(user.Id);
        Assert.NotNull(watchlist);
        Assert.Empty(watchlist.WatchedMovieIds);

        await userService.SetMovieWatchedStateAsync(user.Id, movie1.Id, true);
        watchlist = await userService.GetWatchedMovieIdsAsync(user.Id);
        Assert.NotNull(watchlist);
        Assert.Single(watchlist.WatchedMovieIds);
        Assert.Contains(movie1.Id, watchlist.WatchedMovieIds);

        await userService.SetMovieWatchedStateAsync(user.Id, movie1.Id, false);
        watchlist = await userService.GetWatchedMovieIdsAsync(user.Id);
        Assert.NotNull(watchlist);
        Assert.Empty(watchlist.WatchedMovieIds);

        await userService.SetMovieWatchedStateAsync(user.Id, movie1.Id, true);
        await userService.SetMovieWatchedStateAsync(user.Id, movie2.Id, true);
        watchlist = await userService.GetWatchedMovieIdsAsync(user.Id);
        Assert.NotNull(watchlist);
        Assert.Equal(2, watchlist.WatchedMovieIds.Count);
        Assert.Contains(movie1.Id, watchlist.WatchedMovieIds);
        Assert.Contains(movie2.Id, watchlist.WatchedMovieIds);
    }
}