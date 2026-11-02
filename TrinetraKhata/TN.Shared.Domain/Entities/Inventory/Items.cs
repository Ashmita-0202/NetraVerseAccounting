using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.Entities.StockCenterEntities;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Primitive;

public class Items : Entity
{
    private string? _serialNumber;
    private bool _status;

    protected Items() : base(null) { } // Required for EF Core

    public Items(
        string id,
        string? name,
        decimal? price,
        string? itemGroupId,
        string? unitId,
        string? sellingPrice,
        string? costPrice,
        string? barCodeField,
        //bool status,
        decimal? openingStockQuantity,
        string? hsCode,
        string companyId,
        string createdBy,
        DateTime createdAt,
        string modifiedBy,
        DateTime modifiedAt,
        decimal? minimumLevel,
        bool? hasSerial,
       
        string? conversionFactorId,
        bool? isItems,
        bool? isVatEnables,
        bool? isConversionFactor,
        string? stockCenterId,
        bool? hasExpiryAndManufacture,
        bool? hasBatchNumber,
        List<ManufactureAndExpiry>? manufactureAndExpiries,
        List<BatchNumber>? batchNumbers

    ) : base(id)
    {
        Name = name;
        Price = price;
        ItemGroupId = itemGroupId;
        UnitId = unitId;
        SellingPrice = sellingPrice;
        CostPrice = costPrice;
        BarCodeField = barCodeField;
        ManufacturingAndExpiries = manufactureAndExpiries;
        BatchNumbers = batchNumbers;
        OpeningStockQuantity = openingStockQuantity;

        //if (status)
        //{
        //    _serialNumber = serialNumberGeneratorService.GenerateSerialNumber();
        //}
        //else
        //{
        //    _serialNumber = serialNumber;
        //}

        //Status = status; 
        HsCode = hsCode;
        CompanyId = companyId;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedBy = modifiedBy;
        ModifiedAt = modifiedAt;
        MinimumLevel = minimumLevel;
        HasSerial = hasSerial;
        ConversionFactorId = conversionFactorId;
        IsItems = isItems;
        IsVatEnables = isVatEnables;
        IsConversionFactor = isConversionFactor;
        StockCenterId = stockCenterId;
        HasExpiryAndManufacture = hasExpiryAndManufacture;
        HasBatchNumber = hasBatchNumber;
        ItemInstances = new List<ItemInstance>();
        StockAdjustments = new List<StockAdjustment>();
        StockTransferItems = new List<StockTransferItems>();
    }

    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? ItemGroupId { get; set; }
    public ItemGroup ItemGroup { get; set; }
    public string? UnitId { get; set; }
    public Units Unit { get; set; }
    public string? SellingPrice { get; set; }
    public string? CostPrice { get; set; }
    public string? BarCodeField { get; set; }
    public decimal? OpeningStockQuantity { get; set; }
    public string? HsCode { get;set; }

    public string CompanyId { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public decimal? MinimumLevel { get; set; }
    public bool? HasSerial { get;set; }
    public string? ConversionFactorId { get; set; }
    public ConversionFactor? ConversionFactor { get; set; }
    public bool? IsItems { get; set; }
    public bool? IsConversionFactor { get; set; } = true;

    public bool? IsVatEnables { get; set; }
    public bool? HasExpiryAndManufacture { get; set; }
    public bool? HasBatchNumber { get; set; }

    public ICollection<Inventories> Inventories { get; set; }
    public ICollection<ItemInstance> ItemInstances { get; set; }
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
    public ICollection<ManufactureAndExpiry>? ManufacturingAndExpiries { get; set; }
    public ICollection<BatchNumber>? BatchNumbers { get; set; }

    public string? StockCenterId { get; set; }   
    public StockCenter? StockCenter { get; set; }

    public ICollection<StockTransferItems> StockTransferItems { get; set; }

    //public string? SerialNumber
    //{
    //    get => _serialNumber;
    //    private set => _serialNumber = value;
    //}

    //public bool Status
    //{
    //    get => _status;
    //    private set
    //    {
    //        _status = value;
    //        if (_status && string.IsNullOrEmpty(_serialNumber))
    //        {
    //            throw new InvalidOperationException("Serial Number must be assigned when Status is true.");
    //        }
    //    }
    //}

    //public void UpdateStatus(bool status, ISerialNumberGenerator serialNumberGeneratorService)
    //{
    //    Status = status; // ✅ Uses public property to trigger logic

    //    if (Status)
    //    {
    //        SerialNumber = serialNumberGeneratorService.GenerateSerialNumber(); // ✅ Ensures serial number is assigned
    //    }
    //}
}
