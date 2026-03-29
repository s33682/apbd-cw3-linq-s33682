using System.Runtime.InteropServices.JavaScript;
using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci.Where(s => s.Miasto.Equals("Warsaw"))
            .Select(s => $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}, {s.Miasto}");
    }
    
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci.Select(s => s.Email);
    }
    
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci.OrderBy(s => s.Nazwisko).ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}");
    }

    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        return [DaneUczelni.Przedmioty.Where(s => s.Kategoria.Equals("Analytics"))
            .Select(s => $"{s.Nazwa}, {s.DataStartu}")
            .FirstOrDefault("Brak przedmiotów")];
    }
    
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        return [DaneUczelni.Zapisy.Any(z => z.CzyAktywny.Equals(false))?"Tak":"Nie"];
    }
    
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        return [DaneUczelni.Prowadzacy.All(p => p.Katedra != null) ? "Tak" : "Nie"];
    }
    
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        return [DaneUczelni.Zapisy.Count(z => z.CzyAktywny.Equals(true)).ToString()];
    }
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci.OrderBy(s => s.Miasto)
            .Select(s => s.Miasto)
            .Distinct();
    }
    
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {   
        return DaneUczelni.Zapisy.OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu}, {z.StudentId}, {z.PrzedmiotId}");
    }
    
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty.OrderBy(p => p.Nazwa)
            .Select(p => $"{p.Nazwa}, {p.Kategoria}")
            .Skip(2).Take(2);
    }
    
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, s => s.Id, z => z.StudentId,
            (s, z) => $"{s.Imie}, {s.Nazwisko}, {z.DataZapisu}");
    }
    
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Zapisy.Join(DaneUczelni.Studenci, z => z.StudentId, s => s.Id,
                (z, s) => new { z, s })
            .Join(DaneUczelni.Przedmioty, zs => zs.z.PrzedmiotId, p => p.Id,
                (zs, p) => $"{zs.s.Imie}, {zs.s.Nazwisko}, {p.Nazwa}");
    }
    
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id,
            (z,p) => new {z,p})
            .GroupBy(zp => zp.p.Nazwa)
            .Select(zp => $"{zp.Key}, {zp.Count()}");
    }
    
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id, (z, p) => new {z,p})
            .GroupBy(zp=>zp.p.Nazwa)
            .Where(zp => zp.Average( x => x.z.OcenaKoncowa) != null)
            .Select(zp => $"{zp.Key}, {zp.Average( x => x.z.OcenaKoncowa)}");
    }
    
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy.GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, p) => new {pr, p}
            )
            .Select(prp => $"{prp.pr.Imie}, {prp.pr.Nazwisko},  {prp.p.Count()}");
    }
    
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci.GroupJoin(
                DaneUczelni.Zapisy.Where(z=>z.OcenaKoncowa != null),
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s, z }
            )
            .Where(sz => sz.z.Any())
            .Select(sz => $"{sz.s.Imie}, {sz.s.Nazwisko}, {sz.z.Max(z => z.OcenaKoncowa)}");
    }
    
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s, z }
            )
            .Where(sz => sz.z.Any())
            .Where(sz => sz.z.Count() > 1)
            .Select(sz => $"{sz.s.Imie}, {sz.s.Nazwisko},  {sz.z.Count()}");
    }
    
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .GroupJoin(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { p, z }
            )
            .Where(pz => pz.z.All(z=>z.OcenaKoncowa == null))
            .Select(pz => pz.p.Nazwa);
    }
    
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, p) => new { pr, p }
            )
            .SelectMany(prp => prp.p.DefaultIfEmpty(),
                (prp, p) => new { pr = prp.pr, p })
            .GroupJoin(
                DaneUczelni.Zapisy,
                prp => prp.p == null ? (int?)null : prp.p.Id,
                z => z.PrzedmiotId,
                (prp, z) => new { prp, z }
            )
            .GroupBy(prpz => new { prpz.prp.pr.Imie, prpz.prp.pr.Nazwisko })
            .Select(x =>
            {
                var oceny = x.SelectMany(z => z.z)
                    .Where(z => z.OcenaKoncowa != null)
                    .Select(z => z.OcenaKoncowa);

                var wynik = oceny.Any() ? oceny.Average().ToString() : "Brak";

                return $"{x.Key.Imie}, {x.Key.Nazwisko},  {wynik}";

            });
    }
    
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci.GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s, z }
            )
            .GroupBy(sz => sz.s.Miasto)
            .OrderByDescending(x => x.SelectMany(sz => sz.z).Count())
            .Select(x => $"{x.Key},  {x.SelectMany(sz => sz.z).Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
