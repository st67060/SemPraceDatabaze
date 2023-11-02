DECLARE
  v_table_name VARCHAR2(30);
  v_sequence_name VARCHAR2(30);
BEGIN
  FOR tables IN (SELECT table_name FROM user_tables) LOOP
    v_table_name := tables.table_name;

    -- Zkontrolujte, zda tabulka obsahuje sloupec "id"
    BEGIN
      EXECUTE IMMEDIATE 'SELECT id FROM ' || v_table_name || ' WHERE ROWNUM = 1';
      
      -- Pokud výše uvedený SELECT proběhne bez výjimky, znamená to, že tabulka obsahuje sloupec "id"
      v_sequence_name := v_table_name || '_SEQ'; -- Jméno sekvence bude název tabulky s příponou '_SEQ'
      
      -- Vytvoření sekvence
      EXECUTE IMMEDIATE 'CREATE SEQUENCE ' || v_sequence_name ||
                       ' START WITH 1 INCREMENT BY 1 NOCACHE NOCYCLE';

      -- Nastavení výchozího umělého primárního klíče pro tabulku
      EXECUTE IMMEDIATE 'ALTER TABLE ' || v_table_name ||
                       ' MODIFY id DEFAULT ' || v_sequence_name || '.NEXTVAL';
    EXCEPTION
      WHEN OTHERS THEN
        NULL;
    END;
  END LOOP;
END;
/