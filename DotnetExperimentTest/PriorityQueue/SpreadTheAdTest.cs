
namespace DotnetExperimentTest.PriorityQueue;

public class SpreadTheAdTest
{
    [Fact]
    public void JustOneOrderTest()
    {
        var order1 = new AdOrder
        {
            ID = 1,
            Advertisements = [
                new AdOrder.Advertisement
                {
                    Variant = "A",
                    Quantity = 10
                },
                new AdOrder.Advertisement
                {
                    Variant = "B",
                    Quantity = 15
                },
                new AdOrder.Advertisement
                {
                    Variant = "C",
                    Quantity = 20
                }
            ]
        };

        var adSequence = ScheduleAds(order1);

        Assert.Equal(order1.Advertisements.Sum(a => a.Quantity), adSequence.Count());
        Assert.Equal(
            order1.Advertisements.First().Quantity,
            adSequence.Count(a => a == order1.Advertisements.First().Variant));
    }

    internal class AdOrder
    {
        public int ID { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; } = [];

        public class Advertisement
        {
            public string Variant { get; set; }
            public int Quantity { get; set; }
        }
    }

    private IEnumerable<string> ScheduleAds(AdOrder order)
    {
        var adCount = order.Advertisements.Sum(ad => ad.Quantity);
        var remaining = order.Advertisements
            .ToDictionary(
                a => (a.Variant, Proportion: (float)a.Quantity / adCount),
                a => a.Quantity);
        var sequence = new List<string>();

        for (var i = 0; i < adCount; i++)
        {
            var remainingCount = remaining.Values.Sum();
            var differences = remaining.Keys
                .Select(r => (r.Variant, Diff: (float)remaining[r] / remainingCount - r.Proportion))
                .OrderByDescending(r => r.Diff)
                .ToList();
            var chosenVariant = differences.First().Variant;
            sequence.Add(chosenVariant);
            remaining[remaining.Keys.First(k => k.Variant == chosenVariant)] -= 1;
        }

        return sequence;
    }

    [Fact]
    public void MultipleOrdersTest()
    {
        var order1 = new AdOrder
        {
            ID = 1,
            Advertisements = [
                new AdOrder.Advertisement
                {
                    Variant = "A",
                    Quantity = 10
                },
                new AdOrder.Advertisement
                {
                    Variant = "B",
                    Quantity = 15
                }
            ]
        };

        var order2 = new AdOrder
        {
            ID = 2,
            Advertisements =
            [
                new AdOrder.Advertisement
                {
                    Variant = "C",
                    Quantity = 20
                },
                new AdOrder.Advertisement
                {
                    Variant= "D",
                    Quantity = 25
                },
                new AdOrder.Advertisement{
                    Variant= "E",
                    Quantity = 30
                }
            ]
        };


    }
}