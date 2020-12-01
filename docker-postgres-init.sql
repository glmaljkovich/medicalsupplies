create database dbnetcore;
create user netcoreuser with encrypted password 'netcorepass';
grant all privileges on database dbnetcore to netcoreuser;