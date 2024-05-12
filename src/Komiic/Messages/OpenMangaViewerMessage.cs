using Komiic.Core.Contracts.Model;

namespace Komiic.Messages;

public record OpenMangaViewerMessage(MangaInfo MangaInfo, ChaptersByComicId Chapter);