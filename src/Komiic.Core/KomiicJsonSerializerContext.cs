using System.Text.Json.Serialization;
using Komiic.Core.Contracts.Model;

namespace Komiic.Core;

[JsonSerializable(typeof(ErrorInfo))]
[JsonSerializable(typeof(ResponseData))]
// [JsonSerializable(typeof(ResponseData<>))]
[JsonSerializable(typeof(TokenResponseData))]
[JsonSerializable(typeof(AccountData))]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(FavoriteNewUpdatedData))]
[JsonSerializable(typeof(AuthorSimple))]
[JsonSerializable(typeof(Categories))]
[JsonSerializable(typeof(MangaInfo))]
[JsonSerializable(typeof(RecentUpdateData))]
[JsonSerializable(typeof(HotComicsData))]
[JsonSerializable(typeof(ImageLimit))]
[JsonSerializable(typeof(GetImageLimitData))]
[JsonSerializable(typeof(ComicByIdData))]
[JsonSerializable(typeof(ComicByIdsData))]
[JsonSerializable(typeof(RecommendComicByIdData))]
[JsonSerializable(typeof(MessageCountByComicIdData))]
[JsonSerializable(typeof(LastMessageByComicIdData))]
[JsonSerializable(typeof(LastMessageByComicId))]
[JsonSerializable(typeof(ChaptersByComicIdData))]
[JsonSerializable(typeof(ChaptersByComicId))]
[JsonSerializable(typeof(ImagesByChapterIdData))]
[JsonSerializable(typeof(ImagesByChapterId))]
[JsonSerializable(typeof(AllCategoryData))]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(ComicByCategoryData))]
[JsonSerializable(typeof(AllAuthorsData))]
[JsonSerializable(typeof(Author))]
[JsonSerializable(typeof(ComicsByAuthorData))]

[JsonSerializable(typeof(LoginData))]


[JsonSerializable(typeof(QueryData))]

// [JsonSerializable(typeof(QueryData<>))]
[JsonSerializable(typeof(QueryData<PaginationVariables>))]
[JsonSerializable(typeof(QueryData<ComicIdVariables>))]
[JsonSerializable(typeof(QueryData<ComicIdsVariables>))]
[JsonSerializable(typeof(QueryData<ChapterIdVariables>))]
[JsonSerializable(typeof(QueryData<CategoryIdPaginationVariables>))]
[JsonSerializable(typeof(QueryData<AuthorIdVariables>))]

[JsonSerializable(typeof(Pagination))]
[JsonSerializable(typeof(PaginationVariables))]
[JsonSerializable(typeof(ComicIdVariables))]
[JsonSerializable(typeof(ComicIdsVariables))]
[JsonSerializable(typeof(ChapterIdVariables))]
[JsonSerializable(typeof(CategoryIdPaginationVariables))]
[JsonSerializable(typeof(AuthorIdVariables))]
public partial class KomiicJsonSerializerContext : JsonSerializerContext;
