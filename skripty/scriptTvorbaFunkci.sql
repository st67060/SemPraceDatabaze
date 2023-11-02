  -- funkce pro najiti nejfrekventovanějšího znaku v řetězci
  CREATE OR REPLACE FUNCTION hledani_nejcastejsiho_znaku(vstupni_retezec VARCHAR2) RETURN CHAR IS
  nejcastejsi CHAR;
  max_pocet NUMBER := 0;
BEGIN
  FOR i IN 1..LENGTH(vstupni_retezec) LOOP
    DECLARE
      aktualni_znak CHAR := SUBSTR(vstupni_retezec, i, 1);
      aktualni_pocet NUMBER := 0;
    BEGIN
      FOR j IN 1..LENGTH(vstupni_retezec) LOOP
        IF SUBSTR(vstupni_retezec, j, 1) = aktualni_znak THEN
          aktualni_pocet := aktualni_pocet + 1;
        END IF;
      END LOOP;
      IF aktualni_pocet > max_pocet THEN
        nejcastejsi := aktualni_znak;
        max_pocet := aktualni_pocet;
      END IF;
    END;
  END LOOP;
  RETURN nejcastejsi;
END hledani_nejcastejsiho_znaku;
--funkce pro generovani nahodneho hesla s omezenim na urcite znaky
CREATE OR REPLACE FUNCTION generovani_nahodneho_hesla(
  delka NUMBER,
  obsazene_znaky VARCHAR2,
  vyloucene_znaky VARCHAR2
) RETURN VARCHAR2 IS
  znaky VARCHAR2(100) := obsazene_znaky;
  znaky_vyloucene VARCHAR2(100) := vyloucene_znaky;
  heslo VARCHAR2(100) := '';
  mozna_delka NUMBER := LENGTH(znaky);
  i NUMBER;
  nahodne_cislo NUMBER;
  nova_delka NUMBER := delka; -- Nová proměnná pro délku
BEGIN
  IF LENGTH(znaky) = 0 THEN
    RETURN NULL;
  END IF;

  IF LENGTH(znaky_vyloucene) > 0 THEN
    FOR i IN 1..LENGTH(znaky_vyloucene) LOOP
      znaky := REPLACE(znaky, SUBSTR(znaky_vyloucene, i, 1), '');
    END LOOP;
    mozna_delka := LENGTH(znaky);
  END IF;

  IF nova_delka > mozna_delka THEN
    nova_delka := mozna_delka; -- Použít novou proměnnou
  END IF;

  FOR i IN 1..nova_delka LOOP
    nahodne_cislo := CEIL(DBMS_RANDOM.VALUE(1, mozna_delka));
    heslo := heslo || SUBSTR(znaky, nahodne_cislo, 1);
    znaky := REPLACE(znaky, SUBSTR(znaky, nahodne_cislo, 1), '');
    mozna_delka := LENGTH(znaky);
  END LOOP;

  RETURN heslo;
END generovani_nahodneho_hesla;
-- funkce pro spocitani rpacovních dnu mezi dvěma daty
CREATE OR REPLACE FUNCTION pocet_pracovnich_dnu(od_datum DATE, do_datum DATE) RETURN NUMBER IS
  pocet_dnu NUMBER := 0;
  aktualni_datum DATE := od_datum;
BEGIN
  WHILE aktualni_datum <= do_datum LOOP
    IF TO_CHAR(aktualni_datum, 'DY', 'NLS_DATE_LANGUAGE=AMERICAN') NOT IN ('SAT', 'SUN') THEN
      pocet_dnu := pocet_dnu + 1;
    END IF;
    aktualni_datum := aktualni_datum + 1; -- Přejít na další den
  END LOOP;
  RETURN pocet_dnu;
END pocet_pracovnich_dnu;