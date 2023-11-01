-- Generated by Oracle SQL Developer Data Modeler 23.1.0.087.0806
--   at:        2023-11-01 17:11:07 SE�
--   site:      Oracle Database 11g
--   type:      Oracle Database 11g



-- predefined type, no DDL - MDSYS.SDO_GEOMETRY

-- predefined type, no DDL - XMLTYPE

CREATE TABLE adresy (
    id            NUMBER NOT NULL,
    mesto         VARCHAR2(64) NOT NULL,
    psc           NUMBER NOT NULL,
    cislo_popisne NUMBER NOT NULL,
    stat          VARCHAR2(64) NOT NULL,
    ulice         VARCHAR2(64) NOT NULL
);

ALTER TABLE adresy ADD CONSTRAINT adresy_pk PRIMARY KEY ( id );

CREATE TABLE alergie (
    id    NUMBER NOT NULL,
    nazev VARCHAR2(128) NOT NULL
);

ALTER TABLE alergie ADD CONSTRAINT alergie_pk PRIMARY KEY ( id );

CREATE TABLE alergie_zdrav_karta (
    alergie_id         NUMBER NOT NULL,
    zdravotni_karta_id NUMBER NOT NULL
);

ALTER TABLE alergie_zdrav_karta ADD CONSTRAINT alergie_zdrav_karta_pk PRIMARY KEY ( alergie_id,
                                                                                    zdravotni_karta_id );

CREATE TABLE anamnezy (
    id    NUMBER NOT NULL,
    nemoc VARCHAR2(64) NOT NULL
);

ALTER TABLE anamnezy ADD CONSTRAINT anamnezy_pk PRIMARY KEY ( id );

CREATE TABLE lekarsky_predpis (
    id             NUMBER NOT NULL,
    nazev_leku     VARCHAR2(128) NOT NULL,
    doplatek       NUMBER NOT NULL,
    zamestnanec_id NUMBER NOT NULL,
    pacient_id     NUMBER NOT NULL,
    datum          DATE NOT NULL
);

CREATE UNIQUE INDEX lekarsky_predpis__idx ON
    lekarsky_predpis (
        pacient_id
    ASC );

ALTER TABLE lekarsky_predpis ADD CONSTRAINT lekarsky_predpis_pk PRIMARY KEY ( id );

CREATE TABLE navstevy (
    id         NUMBER NOT NULL,
    datum      DATE NOT NULL,
    poznamky   VARCHAR2(512),
    pacient_id NUMBER NOT NULL
);

ALTER TABLE navstevy ADD CONSTRAINT navstevy_pk PRIMARY KEY ( id );

CREATE TABLE pacient (
    id                  NUMBER NOT NULL,
    jmeno               VARCHAR2(32) NOT NULL,
    prijmeni            VARCHAR2(64) NOT NULL,
    rodne_cislo         NUMBER(10) NOT NULL,
    pohlavi             VARCHAR2(32) NOT NULL,
    narozeni            DATE NOT NULL,
    telefon             NUMBER(12) NOT NULL,
    email               VARCHAR2(64) NOT NULL,
    zdravotni_karta_id  NUMBER NOT NULL,
    adresy_id           NUMBER NOT NULL,
    prac_neschponost_id NUMBER,
    pojistovny_id       NUMBER NOT NULL
);

CREATE UNIQUE INDEX pacient__idx ON
    pacient (
        zdravotni_karta_id
    ASC );

CREATE UNIQUE INDEX pacient__idx ON
    pacient (
        pojistovny_id
    ASC );

ALTER TABLE pacient ADD CONSTRAINT pacient_pk PRIMARY KEY ( id );

CREATE TABLE pojistovny (
    id      NUMBER NOT NULL,
    nazev   VARCHAR2(32) NOT NULL,
    zkratka VARCHAR2(16) NOT NULL
);

ALTER TABLE pojistovny ADD CONSTRAINT pojistovny_pk PRIMARY KEY ( id );

CREATE TABLE prac_neschponost (
    id             NUMBER NOT NULL,
    zacatek        DATE NOT NULL,
    konec          DATE NOT NULL,
    zamestnani     VARCHAR2(64) NOT NULL,
    duvod          VARCHAR2(64) NOT NULL,
    zamestnanec_id NUMBER NOT NULL
);

ALTER TABLE prac_neschponost ADD CONSTRAINT prac_neschponost_pk PRIMARY KEY ( id );

CREATE TABLE rezervace (
    id              NUMBER NOT NULL,
    poznamky        VARCHAR2(512),
    datum           DATE NOT NULL,
    problem_nalezen CHAR(1) NOT NULL,
    pacient_id      NUMBER NOT NULL,
    zamestnanec_id  NUMBER NOT NULL
);

CREATE UNIQUE INDEX rezervace__idx ON
    rezervace (
        zamestnanec_id
    ASC );

ALTER TABLE rezervace ADD CONSTRAINT rezervace_pk PRIMARY KEY ( id );

CREATE TABLE rezervace_zakrok (
    rezervace_id NUMBER NOT NULL,
    zakrok_id    NUMBER NOT NULL
);

ALTER TABLE rezervace_zakrok ADD CONSTRAINT rezervace_zakrok_pk PRIMARY KEY ( rezervace_id,
                                                                              zakrok_id );

CREATE TABLE role (
    id    NUMBER NOT NULL,
    nazev VARCHAR2(32) NOT NULL
);

ALTER TABLE role ADD CONSTRAINT role_pk PRIMARY KEY ( id );

CREATE TABLE uzivatel (
    id             NUMBER NOT NULL,
    zamestnanec_id NUMBER NOT NULL,
    heslo          VARCHAR2(128) NOT NULL,
    jmeno          VARCHAR2(64) NOT NULL
);

CREATE UNIQUE INDEX uzivatel__idx ON
    uzivatel (
        zamestnanec_id
    ASC );

ALTER TABLE uzivatel ADD CONSTRAINT uzivatel_pk PRIMARY KEY ( id );

CREATE TABLE zakrok (
    id               NUMBER NOT NULL,
    nazev            VARCHAR2(128) NOT NULL,
    cena             NUMBER NOT NULL,
    hradi_pojistovna CHAR(1) NOT NULL,
    postup           VARCHAR2(1024)
);

ALTER TABLE zakrok ADD CONSTRAINT zakrok_pk PRIMARY KEY ( id );

CREATE TABLE zamestnanec (
    id        NUMBER NOT NULL,
    jmeno     VARCHAR2(32) NOT NULL,
    prijmeni  VARCHAR2(64) NOT NULL,
    nastup    DATE NOT NULL,
    role_id   NUMBER NOT NULL,
    zakrok_id NUMBER,
    fotka     BLOB
);

ALTER TABLE zamestnanec ADD CONSTRAINT zamestnanec_pk PRIMARY KEY ( id );

CREATE TABLE zdrav_karta_zakrok (
    zdravotni_karta_id NUMBER NOT NULL,
    zakrok_id          NUMBER NOT NULL
);

ALTER TABLE zdrav_karta_zakrok ADD CONSTRAINT zdrav_karta_zakrok_pk PRIMARY KEY ( zdravotni_karta_id,
                                                                                  zakrok_id );

CREATE TABLE zdravotni_karta (
    id          NUMBER NOT NULL,
    kouri       CHAR(1) NOT NULL,
    tehotenstvi CHAR(1) NOT NULL,
    alkohol     CHAR(1) NOT NULL,
    sport       VARCHAR2(128),
    nemoc       VARCHAR2(128) NOT NULL,
    nahrady     NUMBER NOT NULL,
    plomby      NUMBER NOT NULL,
    anamnezy_id NUMBER NOT NULL
);

ALTER TABLE zdravotni_karta ADD CONSTRAINT zdravotni_karta_pk PRIMARY KEY ( id );

ALTER TABLE alergie_zdrav_karta
    ADD CONSTRAINT alergie_zdrav_karta_fk FOREIGN KEY ( zdravotni_karta_id )
        REFERENCES zdravotni_karta ( id );

ALTER TABLE lekarsky_predpis
    ADD CONSTRAINT lekarsky_predpis_pacient_fk FOREIGN KEY ( pacient_id )
        REFERENCES pacient ( id );

--  ERROR: FK name length exceeds maximum allowed length(30) 
ALTER TABLE lekarsky_predpis
    ADD CONSTRAINT lek_predpis_zamestnanec_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanec ( id );

ALTER TABLE navstevy
    ADD CONSTRAINT navstevy_pacient_fk FOREIGN KEY ( pacient_id )
        REFERENCES pacient ( id );

ALTER TABLE pacient
    ADD CONSTRAINT pacient_adresy_fk FOREIGN KEY ( adresy_id )
        REFERENCES adresy ( id );

ALTER TABLE pacient
    ADD CONSTRAINT pacient_pojistovny_fk FOREIGN KEY ( pojistovny_id )
        REFERENCES pojistovny ( id );

ALTER TABLE pacient
    ADD CONSTRAINT pacient_prac_neschponost_fk FOREIGN KEY ( prac_neschponost_id )
        REFERENCES prac_neschponost ( id );

ALTER TABLE pacient
    ADD CONSTRAINT pacient_zdravotni_karta_fk FOREIGN KEY ( zdravotni_karta_id )
        REFERENCES zdravotni_karta ( id );

--  ERROR: FK name length exceeds maximum allowed length(30) 
ALTER TABLE prac_neschponost
    ADD CONSTRAINT prac_nesch_zamestnanec_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanec ( id );

ALTER TABLE rezervace
    ADD CONSTRAINT rezervace_pacient_fk FOREIGN KEY ( pacient_id )
        REFERENCES pacient ( id );

ALTER TABLE rezervace_zakrok
    ADD CONSTRAINT rezervace_zakrok_rezervace_fk FOREIGN KEY ( rezervace_id )
        REFERENCES rezervace ( id );

ALTER TABLE rezervace_zakrok
    ADD CONSTRAINT rezervace_zakrok_zakrok_fk FOREIGN KEY ( zakrok_id )
        REFERENCES zakrok ( id );

ALTER TABLE rezervace
    ADD CONSTRAINT rezervace_zamestnanec_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanec ( id );

ALTER TABLE uzivatel
    ADD CONSTRAINT uzivatel_zamestnanec_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanec ( id );

ALTER TABLE zdrav_karta_zakrok
    ADD CONSTRAINT zakrok_zdrav_karta_fk FOREIGN KEY ( zdravotni_karta_id )
        REFERENCES zdravotni_karta ( id );

ALTER TABLE zamestnanec
    ADD CONSTRAINT zamestnanec_role_fk FOREIGN KEY ( role_id )
        REFERENCES role ( id );

ALTER TABLE zamestnanec
    ADD CONSTRAINT zamestnanec_zakrok_fk FOREIGN KEY ( zakrok_id )
        REFERENCES zakrok ( id );

ALTER TABLE alergie_zdrav_karta
    ADD CONSTRAINT zdrav_karta_alergie_fk FOREIGN KEY ( alergie_id )
        REFERENCES alergie ( id );

ALTER TABLE zdrav_karta_zakrok
    ADD CONSTRAINT zdrav_karta_zakrok_fk FOREIGN KEY ( zakrok_id )
        REFERENCES zakrok ( id );

ALTER TABLE zdravotni_karta
    ADD CONSTRAINT zdravotni_karta_anamnezy_fk FOREIGN KEY ( anamnezy_id )
        REFERENCES anamnezy ( id );



