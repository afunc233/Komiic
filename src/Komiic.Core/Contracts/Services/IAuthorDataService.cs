﻿using Komiic.Core.Contracts.Models;

namespace Komiic.Core.Contracts.Services;

public interface IAuthorDataService
{
    Task<ApiResponseData<List<MangaInfo>>> GetComicsByAuthor(string authorId);


    Task<ApiResponseData<List<Author>>> GetAllAuthors(int pageIndex, string orderBy = "DATE_UPDATED");
}