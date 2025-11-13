using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Services.Mod;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalSICCcaseCS;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class MedicalSiccCaseAfterDbLoad(
    DatabaseServer databaseServer,
    CustomItemService customItemService) : IOnLoad
{
    private readonly MiccConfig _config = MiccConfig.Load();
    private readonly string _itemId = "674f974b8c797c96be0b096c"; // Medical SICC mongo id

    public Task OnLoad()
    {
        // Build minimal clone details (grid + prefab left as original SICC)
        var miccClone = new NewItemFromCloneDetails
        {
            ItemTplToClone = ItemTpl.CONTAINER_SICC,
            ParentId = "5795f317245977243854e041",
            NewId = _itemId,
            FleaPriceRoubles = _config.Price,
            HandbookPriceRoubles = _config.Price,
            HandbookParentId = "5b5f6fa186f77409407a7eb7",
            Locales = new Dictionary<string, LocaleDetails>
            {
                {"en", new LocaleDetails
                    {
                        Name = "Medical SICC case",
                        ShortName = "M I C C",
                        Description = "A SICC case for medical items."
                    }
                }
            },
            OverrideProperties = new TemplateItemProperties
            {
                Name = "Medical SICC",
                ShortName = "M I C C",
                Description = "SICC case for medical items."
            }
        };

        customItemService.CreateItemFromClone(miccClone);
        var items = databaseServer.GetTables().Templates.Items;
        if (items.ContainsKey(new MongoId(_itemId)))
        {
            System.Console.WriteLine("[MedicalSICCcaseCS] Medical SICC item cloned successfully.");
        }
        else
        {
            System.Console.WriteLine("[MedicalSICCcaseCS] Medical SICC item clone FAILED.");
        }
        return Task.CompletedTask;
    }
}
