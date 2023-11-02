DECLARE
  v_sql VARCHAR2(200);
BEGIN
  FOR c in (SELECT constraint_name, table_name, r_constraint_name FROM user_constraints WHERE constraint_type = 'R') LOOP
    v_sql := 'ALTER TABLE ' || c.table_name || ' DROP CONSTRAINT ' || c.constraint_name;
    EXECUTE IMMEDIATE v_sql;
  END LOOP;

  FOR t in (SELECT table_name FROM user_tables) LOOP
    v_sql := 'DROP TABLE ' || t.table_name || ' CASCADE CONSTRAINTS';
    EXECUTE IMMEDIATE v_sql;
  END LOOP;
END;
/