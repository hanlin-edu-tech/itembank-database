using ItemBank.Database.Core.Configuration.BsonSerializers;
using ItemBank.Database.Core.Configuration.BsonSerializers.Abstractions;
using ItemBank.Database.Core.Schema.Enums;
using ItemBank.Database.Core.Schema.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace ItemBank.Database.Core.Configuration;

/// <summary>
/// MongoDB 相關的 DI 擴展方法
/// </summary>
public static class MongoDbExtensions
{
    /// <summary>
    /// 將 DbContext 及索引初始化服務註冊到 DI 容器
    /// </summary>
    /// <param name="services">服務集合</param>
    /// <param name="configure">配置委派</param>
    /// <returns>服務集合</returns>
    public static IServiceCollection AddMongoDbContext(
        this IServiceCollection services,
        Action<MongoDbContextOptionsBuilder> configure)
    {
        RegisterSerializers();
        var builder = new MongoDbContextOptionsBuilder();
        configure(builder);
        var options = builder.Build();

        services.AddSingleton(options);
        services.AddSingleton<DbContext>();

        // 如果啟用自動建立索引，註冊後台服務
        if (options.AutoCreateIndexes)
        {
            services.AddHostedService<IndexInitializationService>();
        }

        return services;
    }

    public static void RegisterSerializers()
    {
        // 註冊 String-based Id 序列化器
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<SubjectId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ProductId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ProductContentId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ProductSectionId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DimensionId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DimensionValueId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<VolumeId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<SourceId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<SourceValueId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<TextbookContentId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<TextbookSectionId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<UserTypeValueId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DocumentItemId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ItemId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<BodyOfKnowledgeId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<VersionId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<CatalogGroupId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<UserId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<UserTypeId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<RepositoryId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DocumentId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<CatalogId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DifficultyId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<DocumentRepoId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ItemIssueId>());
        BsonSerializer.RegisterSerializer(new StringBasedIdSerializer<ValidationTargetId>());

        // 註冊 ObjectId-based Id 序列化器
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<ItemYearDimensionValueId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<TaskId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<DuplicateDetectionRecordId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<ExportTaskId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<ItemMergeHistoryId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<UserConversationId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<ConversationMessageId>());
        BsonSerializer.RegisterSerializer(new ObjectIdBasedIdSerializer<PackageId>());

        // 註冊 Enum Serializer
        BsonSerializer.RegisterSerializer(new EnumSerializer<DimensionType>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<ExportType>(EnumSerializationType.PascalCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<ExportArchiveMode>(EnumSerializationType.PascalCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<BatchProcessingStatus>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<ConversationRole>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<MetadataType>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<ProcessingType>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<CopyrightType>(EnumSerializationType.Integer));
        BsonSerializer.RegisterSerializer(new EnumSerializer<UsageType>(EnumSerializationType.CamelCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<TermEnum>(EnumSerializationType.PascalCase));
        BsonSerializer.RegisterSerializer(new EnumSerializer<SemesterEnum>(EnumSerializationType.PascalCase));

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", conventionPack, _ => true);
        var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register(
            "Ignore Extra Elements for Models",
            pack,
            t => t.FullName?.StartsWith("ItemBank.Database.Core.Schema.Collections") is true
        );
    }
}