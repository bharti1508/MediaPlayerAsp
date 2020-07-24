#include<stdio.h>
#define M 5
#define N 5

void printMatrix(int a[M][N])//int a[M][N])
{
	int i,j;
	//int a[M][N]={0};
  printf("\nPrint Matrix is :\n");
	for(i=0;i<M;i++)
	{
		for(j=0;j<N;j++)
		{
			printf("%d\t",a[i][j]);
		}
		printf("\n");
	}
		printf("\n");
}

void tornatoPrint()
{
	printf("\nIn Tornato Matrix is :\n");
		int i,j,l=0,k=0,p=0;
		int a[M][N]={0};
		int m=M;
		int n=N;
		
		printMatrix(a);//a[M][N]);
		
		  /* k - starting row index  
        m - ending row index  
        l - starting column index  
        n - ending column index  
        i - iterator  
		*/
		while(k<m && l<n)
		{
			for(i=l;i<n;i++)
			{
				a[k][i]=p++;
			}
			k++;
		
				//printMatrix(a);
		
			for(i=k;i<m;i++)
			{
				a[i][n-1]=p++;
			}
			n--;
		
				//printMatrix(a);
				
			for(i=n-1;i>=l;i--)
			{
				a[m - 1][i]=p++;
			}
			m--;
		
				//printMatrix(a);
				
			for(i=m-1;i>=k;i--)
			{
				a[i][l]=p++;
			}
			l++;
		}
		
			printMatrix(a);
		printf("\n");
}
void main()
{
	printf("\n");
	tornatoPrint();
	printf("\n");
}