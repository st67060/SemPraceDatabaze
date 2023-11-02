       --Pohled 1 : zobrazí pacienty a jejich první a poslední návštevu
CREATE OR REPLACE VIEW v_zdravotni_karta_navstevy AS
SELECT
  p.id AS pacient_id,
  p.jmeno AS pacient_jmeno,
  p.prijmeni AS pacient_prijmeni,
  zk.id AS zdravotni_karta_id,
  zk.kouri AS pacient_kouri,
  zk.tehotenstvi AS pacient_tehotenstvi,
  zk.alkohol AS pacient_alkohol,
  COUNT(n.id) AS pocet_navstev,
  MIN(n.datum) AS prvni_navsteva,
  MAX(n.datum) AS posledni_navsteva
FROM
  pacient p
  JOIN zdravotni_karta zk ON p.zdravotni_karta_id = zk.id
  LEFT JOIN navstevy n ON p.id = n.pacient_id
GROUP BY
  p.id, p.jmeno, p.prijmeni, zk.id, zk.kouri, zk.tehotenstvi, zk.alkohol;
 --Pohled 2 : zobrazí informace o úhradách od pojistoven za jednotlivé zákroky a pacienty.  
CREATE OR REPLACE VIEW v_uhrady_od_pojistoven AS
SELECT
  p.id AS pacient_id,
  p.jmeno AS pacient_jmeno,
  p.prijmeni AS pacient_prijmeni,
  z.id AS zakrok_id,
  z.nazev AS zakrok_nazev,
  z.cena AS zakrok_cena,
  po.nazev AS pojistovna_nazev,
  COUNT(rz.rezervace_id) AS pocet_rezervaci
FROM
  pacient p
  JOIN rezervace r ON p.id = r.pacient_id
  JOIN rezervace_zakrok rz ON r.id = rz.rezervace_id
  JOIN zakrok z ON rz.zakrok_id = z.id
  JOIN pojistovny po ON p.pojistovny_id = po.id
GROUP BY
  p.id, p.jmeno, p.prijmeni, z.id, z.nazev, z.cena, po.nazev;
 --Pohled 3 : zobrazí komplexní pohled o navsteve a operaci
CREATE OR REPLACE VIEW v_komplexni_navstevy AS
SELECT
  p.id AS pacient_id,
  p.jmeno AS pacient_jmeno,
  p.prijmeni AS pacient_prijmeni,
  n.id AS navsteva_id,
  n.datum AS datum_navstevy,
  z.id AS zakrok_id,
  z.nazev AS nazev_zakroku,
  z.cena AS cena_zakroku,
  a.nemoc AS diagnoza,
  r.poznamky AS poznamky_rezervace
FROM
  pacient p
  JOIN navstevy n ON p.id = n.pacient_id
  LEFT JOIN rezervace r ON n.id = r.id
  LEFT JOIN rezervace_zakrok rz ON r.id = rz.rezervace_id
  LEFT JOIN zakrok z ON rz.zakrok_id = z.id
  LEFT JOIN zdravotni_karta zk ON p.zdravotni_karta_id = zk.id
  LEFT JOIN anamnezy a ON zk.anamnezy_id = a.id;