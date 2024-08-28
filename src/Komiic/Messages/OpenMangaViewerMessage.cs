using Komiic.Core.Contracts.Models;

namespace Komiic.Messages;

public record OpenMangaViewerMessage(MangaInfo MangaInfo, ChaptersByComicId Chapter);