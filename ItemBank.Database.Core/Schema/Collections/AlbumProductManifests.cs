using System.ComponentModel;
using ItemBank.Database.Core.Schema.Attributes;
using ItemBank.Database.Core.Schema.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ItemBank.Database.Core.Schema.Collections;

[CollectionName("AlbumProductManifests")]
[Description("教材產品對照清單")]
public sealed class AlbumProductManifest : IIndexable<AlbumProductManifest>
{
    [Description("備註")]
    public IReadOnlyList<BsonValue>? Comment { get; init; }

    [Description("EducationCode")]
    public required string EducationCode { get; init; }

    [Description("EducationName")]
    public required string EducationName { get; init; }

    [Description("年級代碼")]
    public required string GradeCode { get; init; }

    [Description("GradeName")]
    public required string GradeName { get; init; }

    [Description("Id")]
    public required string Id { get; init; }

    [Description("排序索引")]
    public required int OrderIndex { get; init; }

    [Description("產品代碼")]
    public required string ProductCode { get; init; }

    [Description("產品 Name")]
    public required string ProductName { get; init; }

    [Description("主旨Code")]
    public required string SubjectCode { get; init; }

    [Description("主旨Name")]
    public required string SubjectName { get; init; }

    [Description("冊次代碼")]
    public required string VolumeCode { get; init; }

    [Description("冊次名稱")]
    public required string VolumeName { get; init; }

[BsonId]
    [Description("Id 值")]
    public required string IdValue { get; init; }

    static IReadOnlyList<CreateIndexModel<AlbumProductManifest>> IIndexable<AlbumProductManifest>.CreateIndexModelsWithoutDefault =>
    [
        new(
            Builders<AlbumProductManifest>.IndexKeys.Combine(
                    Builders<AlbumProductManifest>.IndexKeys.Ascending("subjectId"),
                    Builders<AlbumProductManifest>.IndexKeys.Ascending("productCode"),
                    Builders<AlbumProductManifest>.IndexKeys.Ascending("volumeName")
                ),
                new CreateIndexOptions<AlbumProductManifest>
                {
                    Name = "subjectId_1_productCode_1_volumeName_1"
                }
        )
    ];
}
