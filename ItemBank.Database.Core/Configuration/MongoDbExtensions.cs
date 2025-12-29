using ItemBank.Database.Core.Configuration.BsonSerializers;
using ItemBank.Database.Core.Schema.Enums;
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

    private static void RegisterSerializers()
    {
        // 註冊所有值物件序列化器
        BsonSerializer.RegisterSerializer(new SubjectIdSerializer());
        BsonSerializer.RegisterSerializer(new ProductIdSerializer());
        BsonSerializer.RegisterSerializer(new ProductContentIdSerializer());
        BsonSerializer.RegisterSerializer(new ProductSectionIdSerializer());
        BsonSerializer.RegisterSerializer(new DimensionIdSerializer());
        BsonSerializer.RegisterSerializer(new DimensionValueIdSerializer());
        BsonSerializer.RegisterSerializer(new VolumeIdSerializer());
        BsonSerializer.RegisterSerializer(new SourceIdSerializer());
        BsonSerializer.RegisterSerializer(new SourceValueIdSerializer());
        BsonSerializer.RegisterSerializer(new TextbookContentIdSerializer());
        BsonSerializer.RegisterSerializer(new TextbookSectionIdSerializer());
        BsonSerializer.RegisterSerializer(new UserTypeValueIdSerializer());
        BsonSerializer.RegisterSerializer(new ItemYearDimensionValueIdSerializer());
        BsonSerializer.RegisterSerializer(new DocumentItemIdSerializer());
        BsonSerializer.RegisterSerializer(new ItemIdSerializer());
        BsonSerializer.RegisterSerializer(new BodyOfKnowledgeIdSerializer());
        BsonSerializer.RegisterSerializer(new VersionIdSerializer());
        BsonSerializer.RegisterSerializer(new CatalogGroupIdSerializer());
        BsonSerializer.RegisterSerializer(new UserIdSerializer());
        BsonSerializer.RegisterSerializer(new UserTypeIdSerializer());
        BsonSerializer.RegisterSerializer(new TaskIdSerializer());
        BsonSerializer.RegisterSerializer(new RepositoryIdSerializer());
        BsonSerializer.RegisterSerializer(new DocumentIdSerializer());
        BsonSerializer.RegisterSerializer(new CatalogIdSerializer());
        BsonSerializer.RegisterSerializer(new DifficultyIdSerializer());
        BsonSerializer.RegisterSerializer(new DocumentRepoIdSerializer());
        BsonSerializer.RegisterSerializer(new DuplicateDetectionRecordIdSerializer());
        BsonSerializer.RegisterSerializer(new ExportTaskIdSerializer());
        BsonSerializer.RegisterSerializer(new ItemIssueIdSerializer());
        BsonSerializer.RegisterSerializer(new ItemMergeHistoryIdSerializer());
        BsonSerializer.RegisterSerializer(new UserConversationIdSerializer());
        BsonSerializer.RegisterSerializer(new ConversationMessageIdSerializer());
        BsonSerializer.RegisterSerializer(new ValidationTargetIdSerializer());
        BsonSerializer.RegisterSerializer(new PackageIdSerializer());

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