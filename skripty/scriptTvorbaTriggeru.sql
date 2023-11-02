--trigger pro kontrolu rezervaci
CREATE OR REPLACE TRIGGER kontrola_prekryvajicich_rezervaci
BEFORE INSERT OR UPDATE ON rezervace
FOR EACH ROW
DECLARE
  pocet_prekryvajicich_rezervaci NUMBER;
BEGIN
  SELECT COUNT(*)
  INTO pocet_prekryvajicich_rezervaci
  FROM rezervace
  WHERE pacient_id = :new.pacient_id
    AND zamestnanec_id = :new.zamestnanec_id
    AND :new.datum >= datum
    AND :new.datum <= datum + INTERVAL '1' HOUR; 

  IF pocet_prekryvajicich_rezervaci > 0 THEN
    RAISE_APPLICATION_ERROR(-20001, 'Nová rezervace překrývá existující rezervace.');
  END IF;
END kontrola_prekryvajicich_rezervaci;

CREATE OR REPLACE TRIGGER generovani_zkratky_zamestnance
BEFORE INSERT ON zamestnanec
FOR EACH ROW
DECLARE
  zkratka VARCHAR2(5);
BEGIN
--trigger pro nastaveni uvodni fotky zamestnance
CREATE OR REPLACE TRIGGER nastaveni_vychozi_fotky
BEFORE INSERT ON zamestnanec
FOR EACH ROW
DECLARE
  vychozi_fotka BLOB;
BEGIN
  IF :new.fotka IS NULL THEN
    vychozi_fotka := utl_raw.cast_to_raw('data:image/svg+xml;utf8;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDEgMTAxIiB4bWw6c3BhY2U9InByZXNlcnZlIj48cGF0aCBkPSJNNTAuNCA1NC41YzEwLjEgMCAxOC4yLTguMiAxOC.4yLTE4LjJTNjAuNSAxOCA1MC40IDE4cy0xOC4yIDguMi0xOC4yIDE4LjIgOC4xIDE4LjMgMTguMiAxOCzem0wLTMxLjdjNy40IDAgMTMuNCA2IDEzLjQgMTMuNHMtNiAxMy.4wLTEzLjQgMTMuNFMzNyA0My43IDM3IDM2LjNzNi0xMy41IDEzLjQtMTMuNXpNMTguOCA4M2g2My40YzEuMyAwIDIuNC0xLjEgMi40LTIuNCAwLTEyLjYtMTAuMy0yMi45LTIyLjktMjIuOUgzOS4zYy0xMi42IDAtMjIuOSAxMC4zLTIyLjkgMjIuOSAwIDEuMyAxLjEgMi40IDIuNCAyLjR6bTIwLjUtMjAuNWgyMi.40YzkuMiAwIDE2LjcgNi.44IDE3LjkgMTUuN0gyMS40YzEuMi04LjkgOC43LTE1LjcgMTcuOS0x5Ljc');
    :new.fotka := vychozi_fotka;
  END IF;
END nastaveni_vychozi_fotky;
