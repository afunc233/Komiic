using Komiic.Core.Contracts.Apis;
using Komiic.Core.Contracts.Models;
using Komiic.Core.Contracts.Services;

namespace Komiic.Core.Services;

internal class MangaDetailDataService(IKomiicQueryApi komiicQueryApi) : IMangaDetailDataService
{
    public async Task<ApiResponseData<MangaInfo?>> GetMangaInfoById(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var variables =
            QueryDataEnum.ComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });
        var mangaInfoByIdData = await komiicQueryApi.GetMangaInfoById(variables, cancellationToken);

        if (mangaInfoByIdData is { Data.ComicById: not null })
        {
            return new ApiResponseData<MangaInfo?>
            {
                Data = mangaInfoByIdData.Data.ComicById,
                ErrorMessage = mangaInfoByIdData.GetMessage()
            };
        }

        return new ApiResponseData<MangaInfo?>
        {
            Data = default,
            ErrorMessage = mangaInfoByIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<int?>> GetMessageCountByComicId(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var variables =
            QueryDataEnum.MessageCountByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var messageCountByComicIdData = await komiicQueryApi.GetMessageCountByComicId(variables, cancellationToken);

        if (messageCountByComicIdData is { Data.MessageCountByComicIdCount: not null })
        {
            return new ApiResponseData<int?>
            {
                Data = messageCountByComicIdData.Data.MessageCountByComicIdCount,
                ErrorMessage = messageCountByComicIdData.GetMessage()
            };
        }

        return new ApiResponseData<int?> { Data = default, ErrorMessage = messageCountByComicIdData.GetMessage() };
    }

    public async Task<ApiResponseData<List<MessagesByComicId>>> GetMessagesByComicId(string comicId, int pageIndex,
        string orderBy = "DATE_UPDATED", CancellationToken? cancellationToken = null)
    {
        const int pageSize = 100;
        var variables =
            QueryDataEnum.GetMessagesByComicId.GetQueryDataWithVariables(new ComicIdPaginationVariables
            {
                ComicId = comicId,
                Pagination = new Pagination
                {
                    Offset = pageIndex * pageSize,
                    Limit = pageSize,
                    OrderBy = orderBy,
                    Asc = true
                }
            });

        var messagesByComicIdData = await komiicQueryApi.GetMessagesByComicId(variables, cancellationToken);
        if (messagesByComicIdData is { Data.GetMessagesByComicId.Count: > 0 })
        {
            return new ApiResponseData<List<MessagesByComicId>>
            {
                Data = messagesByComicIdData.Data.GetMessagesByComicId,
                ErrorMessage = messagesByComicIdData.GetMessage()
            };
        }

        return new ApiResponseData<List<MessagesByComicId>>
        {
            ErrorMessage = messagesByComicIdData.GetMessage(),
            Data = []
        };
    }

    public async Task<ApiResponseData<LastMessageByComicId?>> GetLastMessageByComicId(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var variables =
            QueryDataEnum.LastMessageByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });


        var lastMessageByComicIdData = await komiicQueryApi.GetLastMessageByComicId(variables, cancellationToken);
        if (lastMessageByComicIdData is { Data.LastMessageByComicId: not null })
        {
            return new ApiResponseData<LastMessageByComicId?>
            {
                Data = lastMessageByComicIdData.Data.LastMessageByComicId,
                ErrorMessage = lastMessageByComicIdData.GetMessage()
            };
        }

        return new ApiResponseData<LastMessageByComicId?>
        {
            Data = default,
            ErrorMessage = lastMessageByComicIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<ChaptersByComicId>>> GetChapterByComicId(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var variables =
            QueryDataEnum.ChapterByComicId.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var chapterByComicIdData = await komiicQueryApi.GetChapterByComicId(variables, cancellationToken);
        if (chapterByComicIdData is { Data.ChaptersByComicIdList.Count: > 0 })
        {
            return new ApiResponseData<List<ChaptersByComicId>>
            {
                Data = chapterByComicIdData.Data.ChaptersByComicIdList,
                ErrorMessage = chapterByComicIdData.GetMessage()
            };
        }

        return new ApiResponseData<List<ChaptersByComicId>>
        {
            Data = [],
            ErrorMessage = chapterByComicIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<string>>> GetRecommendComicById(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var variables =
            QueryDataEnum.RecommendComicById.GetQueryDataWithVariables(new ComicIdVariables { ComicId = comicId });

        var recommendComicByIdData = await komiicQueryApi.GetRecommendComicById(variables, cancellationToken);
        if (recommendComicByIdData is { Data.RecommendComicList.Count: > 0 })
        {
            return new ApiResponseData<List<string>>
            {
                Data = recommendComicByIdData.Data.RecommendComicList,
                ErrorMessage = recommendComicByIdData.GetMessage()
            };
        }

        return new ApiResponseData<List<string>>
        {
            Data = [],
            ErrorMessage = recommendComicByIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetRecommendMangaInfosById(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var recommendComicData =
            await GetRecommendComicById(comicId, cancellationToken);
        if (recommendComicData.Data is { Count: > 0 })
        {
            var mangaInfoByIdsData = await GetMangaInfoByIds(cancellationToken, recommendComicData.Data);

            if (mangaInfoByIdsData.Data is { Count: > 0 })
            {
                return mangaInfoByIdsData;
            }
        }

        return new ApiResponseData<List<MangaInfo>> { Data = [], ErrorMessage = recommendComicData.ErrorMessage };
    }

    public async Task<ApiResponseData<List<MangaInfo>>> GetMangaInfoByIds(CancellationToken? cancellationToken = null,
        params List<string> comicIds)
    {
        var variables = QueryDataEnum.ComicByIds.GetQueryDataWithVariables(new ComicIdsVariables
            { ComicIdList = comicIds.ToList() });
        var mangaInfoByIdsData = await komiicQueryApi.GetMangaInfoByIds(variables, cancellationToken);
        if (mangaInfoByIdsData is { Data.ComicByIds.Count: > 0 })
        {
            return new ApiResponseData<List<MangaInfo>>
            {
                Data = mangaInfoByIdsData.Data.ComicByIds,
                ErrorMessage = mangaInfoByIdsData.GetMessage()
            };
        }

        return new ApiResponseData<List<MangaInfo>>
        {
            Data = [],
            ErrorMessage = mangaInfoByIdsData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<Folder>>> GetMyFolders(CancellationToken? cancellationToken = null)
    {
        var queryData = QueryDataEnum.MyFolder.GetQueryData();

        var folderData = await komiicQueryApi.GetMyFolder(queryData, cancellationToken);
        if (folderData is { Data.Folders.Count: > 0 })
        {
            return new ApiResponseData<List<Folder>>
            {
                Data = folderData.Data.Folders,
                ErrorMessage = folderData.GetMessage()
            };
        }

        return new ApiResponseData<List<Folder>>
        {
            Data = [],
            ErrorMessage = folderData.GetMessage()
        };
    }

    public async Task<ApiResponseData<bool?>> RemoveComicToFolder(string folderId, string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData = QueryDataEnum.RemoveComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var removeComicToFolderData = await komiicQueryApi.RemoveComicToFolder(queryData, cancellationToken);

        if (removeComicToFolderData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = removeComicToFolderData.Data.RemoveComicToFolder,
                ErrorMessage = removeComicToFolderData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = removeComicToFolderData.GetMessage()
        };
    }

    public async Task<ApiResponseData<bool?>> AddComicToFolder(string folderId, string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData = QueryDataEnum.AddComicToFolder.GetQueryDataWithVariables(new FolderIdAndComicIdVariables
        {
            FolderId = folderId,
            ComicId = comicId
        });

        var addComicToFolderData = await komiicQueryApi.AddComicToFolder(queryData, cancellationToken);

        if (addComicToFolderData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = addComicToFolderData.Data.AddComicToFolder,
                ErrorMessage = addComicToFolderData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = addComicToFolderData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<string>>> ComicInAccountFolders(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData = QueryDataEnum.ComicInAccountFolders.GetQueryDataWithVariables(new ComicIdVariables
        {
            ComicId = comicId
        });

        var comicInAccountFoldersData = await komiicQueryApi.ComicInAccountFolders(queryData, cancellationToken);

        if (comicInAccountFoldersData is { Data.ComicInAccountFolders.Count: > 0 })
        {
            return new ApiResponseData<List<string>>
            {
                Data = comicInAccountFoldersData.Data.ComicInAccountFolders,
                ErrorMessage = comicInAccountFoldersData.GetMessage()
            };
        }

        return new ApiResponseData<List<string>>
        {
            Data = [],
            ErrorMessage = comicInAccountFoldersData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<LastReadByComicId>>> GetComicsLastRead(
        CancellationToken? cancellationToken = null, params List<string> comicIds)
    {
        var queryData =
            QueryDataEnum.ComicsLastRead.GetQueryDataWithVariables(
                new ComicIdsVariables { ComicIdList = comicIds.ToList() });

        var comicsLastReadData = await komiicQueryApi.GetComicsLastRead(queryData, cancellationToken);
        if (comicsLastReadData is { Data.LastReadByComicIds.Count: > 0 })
        {
            return new ApiResponseData<List<LastReadByComicId>>
            {
                Data = comicsLastReadData.Data.LastReadByComicIds,
                ErrorMessage = comicsLastReadData.GetMessage()
            };
        }

        return new ApiResponseData<List<LastReadByComicId>>
        {
            Data = [],
            ErrorMessage = comicsLastReadData.GetMessage()
        };
    }

    public async Task<ApiResponseData<bool?>> RemoveFavorite(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData =
            QueryDataEnum.RemoveFavorite.GetQueryDataWithVariables(
                new ComicIdVariables { ComicId = comicId });

        var removeFavoriteData = await komiicQueryApi.RemoveFavorite(queryData, cancellationToken);

        if (removeFavoriteData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = removeFavoriteData.Data.RemoveFavorite,
                ErrorMessage = removeFavoriteData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = removeFavoriteData.GetMessage()
        };
    }

    public async Task<ApiResponseData<Favorite>> AddFavorite(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData =
            QueryDataEnum.AddFavorite.GetQueryDataWithVariables(
                new ComicIdVariables { ComicId = comicId });


        var addFavoriteData = await komiicQueryApi.AddFavorite(queryData, cancellationToken);

        if (addFavoriteData is { Data: not null })
        {
            return new ApiResponseData<Favorite>
            {
                Data = addFavoriteData.Data.AddFavorite,
                ErrorMessage = addFavoriteData.GetMessage()
            };
        }

        return new ApiResponseData<Favorite>
        {
            Data = default,
            ErrorMessage = addFavoriteData.GetMessage()
        };
    }

    public async Task<ApiResponseData<bool?>> VoteMessage(string messageId, bool isUp,
        CancellationToken? cancellationToken = null)
    {
        var queryData = QueryDataEnum.VoteMessage.GetQueryDataWithVariables(new VoteMessageVariables
        {
            MessageId = messageId,
            Up = isUp
        });

        var voteMessageData = await komiicQueryApi.VoteMessage(queryData, cancellationToken);
        if (voteMessageData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = voteMessageData.Data.VoteMessage,
                ErrorMessage = voteMessageData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = voteMessageData.GetMessage()
        };
    }

    public async Task<ApiResponseData<List<MessageVotesByComicId>>> MessageVotesByComicId(string comicId,
        CancellationToken? cancellationToken = null)
    {
        var queryData =
            QueryDataEnum.MessageVotesByComicId.GetQueryDataWithVariables(
                new ComicIdVariables { ComicId = comicId });


        var messageVotesByComicIdData = await komiicQueryApi.MessageVotesByComicId(queryData, cancellationToken);
        if (messageVotesByComicIdData is { Data: not null })
        {
            return new ApiResponseData<List<MessageVotesByComicId>>
            {
                Data = messageVotesByComicIdData.Data.MessageVotesByComicId,
                ErrorMessage = messageVotesByComicIdData.GetMessage()
            };
        }

        return new ApiResponseData<List<MessageVotesByComicId>>
        {
            Data = [],
            ErrorMessage = messageVotesByComicIdData.GetMessage()
        };
    }

    public async Task<ApiResponseData<MessagesByComicId>> AddMessageToComic(string comicId,
        string sendMessageText, string? replyToId, CancellationToken? cancellationToken = null)
    {
        await Task.CompletedTask;

        var queryData =
            QueryDataEnum.AddMessageToComic.GetQueryDataWithVariables(
                new AddMessageToComicVariables
                    { ComicId = comicId, Message = sendMessageText, ReplyToId = replyToId ?? "0" });

        var addMessageToComicData = await komiicQueryApi.AddMessageToComic(queryData, cancellationToken);

        if (addMessageToComicData is { Data: not null })
        {
            return new ApiResponseData<MessagesByComicId>
            {
                Data = addMessageToComicData.Data.AddMessageToComic,
                ErrorMessage = addMessageToComicData.GetMessage()
            };
        }

        return new ApiResponseData<MessagesByComicId>
        {
            Data = default,
            ErrorMessage = addMessageToComicData.GetMessage()
        };
    }

    public async Task<ApiResponseData<bool?>> DeleteMessage(string messageId,
        CancellationToken? cancellationToken = null)
    {
        var queryData =
            QueryDataEnum.DeleteMessage.GetQueryDataWithVariables(
                new MessageIdVariables { MessageId = messageId });

        var deleteMessageData = await komiicQueryApi.DeleteMessage(queryData, cancellationToken);

        if (deleteMessageData is { Data: not null })
        {
            return new ApiResponseData<bool?>
            {
                Data = deleteMessageData.Data.DeleteMessage,
                ErrorMessage = deleteMessageData.GetMessage()
            };
        }

        return new ApiResponseData<bool?>
        {
            Data = default,
            ErrorMessage = deleteMessageData.GetMessage()
        };
    }
}