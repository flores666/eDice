namespace AssetCrafterService.Models;

/// <summary>
/// Способы сортировки токенов
/// </summary>
public enum TokensSortOption
{
    /// <summary>
    /// По заголовку А-Я
    /// </summary>
    Asc,
    /// <summary>
    /// По заголовку Я-А
    /// </summary>
    Desc,
    /// <summary>
    /// Сначала подтвержденные
    /// </summary>
    Confirmed,
    /// <summary>
    /// Сначала оффициальные
    /// </summary>
    Official,
    /// <summary>
    /// Сначала старые
    /// </summary>
    OldFirst,
    /// <summary>
    /// Сначала новые
    /// </summary>
    NewFirst
}