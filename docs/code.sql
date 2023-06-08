SELECT u.id, u.name, (
    SELECT count(*)::int
    FROM user_files AS u1
    WHERE u.id = u1.user_id), u0.id IS NULL, u0.id, u0.name
FROM users AS u
LEFT JOIN users AS u0 ON u.parent_id = u0.id
WHERE (@__filter_Name_0 = '' OR strpos(u.name, @__filter_Name_0) > 0) AND u.creation_date >= @__filter_CreatedAfter_1