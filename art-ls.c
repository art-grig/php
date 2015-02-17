//аналог ls -l и ls-lR (а так же ls файлового)
//Компиляция: gcc art-ls.c -o art_ls
//Пример запуска: ./art_ls -lR /home/user/Desktop/




#include <string.h>
#include <stdio.h>
#include <sys/stat.h>
#include <grp.h>
#include <stdlib.h>
#include <dirent.h>
#include <time.h>
#include <pwd.h>

inline void abort_prg(const char* str);
inline char print_type(int m);
inline void set_mode(int m);
inline void file_ls(const char* filename); //для файлов
inline void ls_l(const char* dirname);   //-l
void ls_R(const char *dirname);  //-lR


int main(int argc, const char* argv[])
{
        const char* dirname;
        struct stat st;
        if (argc < 2)   //Работаем с текущей директорией(при этом применяем аналог ls -l)
        {
                dirname = ".";
            	ls_l(dirname);
        		return 0;
        } 	
        else {					
        	if (argc == 2)  //работаем с текущей директорией (взависимости от argv[1] решаем , что делать)
        	{
        		if (!strcmp(argv[1], "-l"))  //если пользователь просит -l 
        		{
        			dirname = ".";
        			ls_l(dirname);
        			return 0;
        		}
        		
        			if (!strcmp(argv[1], "-lR")) //пользователь просит -lR
        			{
        				dirname = "."; 
        				ls_R(dirname);
        				return 0;
        			} 
        			
        		
        			dirname = argv[1];       
        			if (0 == stat(dirname, &st) && '-' == print_type(st.st_mode)) //Если файл 
        			{
        				file_ls(dirname);
        				return 0;
        			}
        	
        			
        				ls_R(dirname);    // если директория - рекурсивный ls (аналог ls -R)
        				return 0; 
        	}	
        	
        	if (argc>2)       //с переданной директорией решаем, что делать
        	{
        		dirname = argv[2];
        		if (!strcmp(argv[1], "-l"))  
        		{
        			ls_l(dirname);
        			return 0;
        		}
        		else 
        		{
        			if (!strcmp(argv[1], "-lR")) 
        			{
        				ls_R(dirname);
        				return 0;
        			} 
        		}
        	}
        }
 
    	return 0;
}



inline void abort_prg(const char* str)
{
        fprintf(stderr, "%s\n", str);
        exit(1);
}

inline char print_type(int m)
{
        if(m & S_IFDIR) return 'd';
        else if(m & S_IFBLK) return 'b';
        else if(m & S_IFCHR) return 'c';
        else if(m & S_IFIFO) return 'p';
        else return '-';
}

inline void set_mode(int m)
{
        if (m & S_IRUSR) printf("r"); else printf("-");
        if (m & S_IWUSR) printf("w"); else printf("-");
        if (m & S_IXUSR) printf("x"); else printf("-");
        if (m & S_IRGRP) printf("r"); else printf("-");
        if (m & S_IWGRP) printf("w"); else printf("-");
        if (m & S_IXGRP) printf("x"); else printf("-");
        if (m & S_IROTH) printf("r"); else printf("-");
        if (m & S_IWOTH) printf("w"); else printf("-");
        if (m & S_IXOTH) printf("x"); else printf("-");

}

inline void file_ls(const char* filename)
{
        struct stat st;

        struct passwd* user;
        struct group* group;
        if (0 == stat(filename, &st))
        {
		int mode = st.st_mode;
        	struct tm* time;
        	char time_str[80];

        	printf("%c", print_type(mode));
        	set_mode(mode);

        	printf(" %d ", (int)st.st_nlink);

        	user = getpwuid(st.st_uid);
        	if (NULL == user)
                	abort_prg("st_uid не найден");
        	printf("%s ", user->pw_name);

        	group = getgrgid(st.st_gid);
        	if (NULL == group)
                	abort_prg("st.gid не найден");
        	printf("%s ", group->gr_name);

        	printf("%d ", (int)st.st_size);

        	time = localtime((const time_t*)&(st.st_mtime));
        	strftime(time_str, 80, "%b %e %H:%M", time);
        	printf("%s ", time_str);

        	printf("%s\n", filename);
        }
}

inline void ls_l(const char* dirname) 
{
	DIR* dir; 
	struct dirent* d; 
	char* dname; 
	int mode; 
	struct stat st; 
	struct passwd *user; 
	struct group *group; 
	struct tm *time; 
	char time_str[80]; 
	int dirsize; 
	dirsize = 0;
	printf("%s:\n", dirname);
	dir = opendir(dirname);
        
        while((d = readdir(dir)) != NULL) 
	{
		dname = (char*) malloc((strlen(dirname) + strlen(d->d_name) + 2)*sizeof(char));
		if (NULL == dname)
			abort_prg("Недостаточно памяти");

		strcpy(dname, dirname);
		strcat(dname, "/");
		strcat(dname, d -> d_name);

		if (stat(dname, &st) || d -> d_name[0] == '.')
			continue;

		mode = st.st_mode;
		if (!(mode & S_IRUSR))
			continue;

		dirsize=dirsize + st.st_size/512;

		free(dname);
	}
        
	closedir(dir);
	printf("Итого: %d\n", dirsize);
	dir = opendir(dirname);
   		     
	while((d = readdir(dir)) != NULL) 
	{
		dname = (char*) malloc((strlen(dirname) + strlen(d->d_name) + 2)*sizeof(char));
		if (NULL == dname)
			abort_prg("Недостаточно памяти");

		strcpy(dname, dirname);
		strcat(dname, "/");
		strcat(dname, d -> d_name);
            
		if (stat(dname, &st) || d -> d_name[0] == '.') 
			continue;

		mode = st.st_mode;
		if (!(mode & S_IRUSR)) 
			continue;
		printf("%c", print_type(mode)); 

		set_mode(mode); 

		printf(" %d ", (int)st.st_nlink);

		user = getpwuid(st.st_uid); 
		printf("%s ", user -> pw_name);

		group = getgrgid(st.st_gid); 
		printf("%s ", group -> gr_name);
		printf("%d ", (int)st.st_size); 

		time = localtime((const time_t*)&(st.st_mtime)); 
		strftime(time_str, 80, "%b %e %H:%M", time);
		printf("%s ", time_str);

		printf(" %s\n", d -> d_name); 
		free(dname);
	}
	printf("\n");
}

void ls_R(const char *dirname) 
{
	DIR* dir; 
	struct dirent* d; 
	dir = opendir(dirname);
	ls_l(dirname); 
        
	while((d = readdir(dir)) != NULL) 
	{
		DIR* theD; 
		char* theDirname; 
		theDirname = (char*) malloc((strlen(dirname) + strlen(d -> d_name) + 2)*sizeof(char));
            	if (NULL == theDirname)
			abort_prg("Недостаточно памяти");

		strcpy(theDirname, dirname);
		strcat(theDirname, "/");
		strcat(theDirname, d -> d_name);

		theD = opendir(theDirname); 
		if(theD != NULL && d -> d_name[0] != '.') 
		{
			closedir(theD);
			ls_R(theDirname);
		}
		free(theDirname);
	}	
}
