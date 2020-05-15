#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <iostream>
#include <fstream>
#include <string.h>
#include <time.h>
#include <algorithm>

using namespace std;

#define NOD 150		// ������ ����
#define DIM 4		// ������ ������
#define K 3			// Ŭ������ ��

struct XV{			//������ �Է¿� ����
	double x[DIM];	// ������ ��
	int real;		// ���� �з���
	int label;		// Ŭ������ �����
};

XV data[NOD];		// ������ ����� ����ü�迭
double cx[K][DIM];	// Ŭ������ �߽ɰ� ���� �迭
double bcx[K][DIM];  // ���� Ŭ������ �߽ɰ� ���� �迭

int fmin(double a[]){	// �ּҰ� ã�� �Լ�
	double min = 9999999999999999999.0;
	int dk;
	for(int i=0;i<K;i++){
		if (a[i] < min){
			dk = i;
			min = a[i];
		}
	}

	return dk;
}

void read_data() {	// ���Ϸ� ���� �����͸� �о� ����ü �迭�� ����
	ifstream in("iris2.dat");

	if (in.is_open()){
		for (int i = 0; i<NOD; i++){
			for(int j=0; j<DIM; j++){
				in >> data[i].x[j];
			}
			in >> data[i].real;
		}
	}
	else {
		cout << "������ ã�� �� �����ϴ�!" << endl;
		exit(-1);
	}

	in.close();
}

void print_data() {	// ���� ��� �����͸� Ŭ������ ���� �Բ� ���

	for (int i = 0; i<NOD; i++){
		for(int j=0; j<DIM; j++){
			printf("%.1f ", data[i].x[j]);
		}
		cout << data[i].real << ' ' << data[i].label << endl;
	}
}

void init_center_forgy(){	// forgy �˰��� ���� �ʱ� �߽ɰ� ���� �Լ�

	int random;

	for(int i=0; i<K; i++){
		random = rand() % NOD;
		for(int j=0;j<DIM;j++){
			cx[i][j] = data[random].x[j];
		}
	}
}

void init_center_RP(){	//  random partition ����� ���� �ʱ� �߽ɰ� ���� �Լ�

	int random;
	double sum[K][DIM]={0};	// ������ ������ �� ����
	int count[K]={0};		// Ŭ�����ͺ� ������ ���� ����

	for(int i=0; i<NOD; i++){
		random = rand() % K;
		data[i].label = random;
	}

	for(int i=0;i<NOD;i++){
		for(int j=0;j<DIM;j++){
			sum[data[i].label][j] += data[i].x[j];
		}
		count[data[i].label]++;
	}

	for(int i=0;i<K;i++){
		for(int j=0;j<DIM;j++){
			cx[i][j] = sum[i][j] / double(count[i]);
		}
	}
}

double distance(XV a, int k){	// k��° Ŭ�����Ϳ� a �����Ϳ��� �Ÿ� ���
	double diffsum = 0;
	for(int i=0;i<DIM;i++){
		diffsum += (a.x[i] - cx[k][i])*(a.x[i] - cx[k][i]);
	}

	return sqrt(diffsum);
}
	
void assign_data(){		
	// Ŭ������ �� �߽ɰ��� �Ÿ��� ���� ���� ������ Ŭ������ ���� �Ҵ��ϴ� �Լ�
	double dis[K]={0};

	for(int i=0;i<NOD;i++){

		for(int j=0;j<K;j++){
			dis[j]= distance(data[i], j);
		}
		data[i].label = fmin(dis);
	}
}

void update_centers(){	
	// �Ҵ�� �����͵��� �̿��Ͽ� ���ο� �߽ɰ��� ����ϴ� �Լ�

	double sum[K][DIM]={0};
	int count[K]={0};

	for(int i=0;i<NOD;i++){
		for(int j=0;j<DIM;j++){
			sum[data[i].label][j] += data[i].x[j];
		}
		count[data[i].label]++;
	}

	for(int i=0;i<K;i++){
		for(int j=0;j<DIM;j++){
			cx[i][j] = sum[i][j] / double(count[i]);
		}
	}

	for(int i=0;i<K;i++){
		cout << count[i] << ' ';
	}
	cout << endl;
}	

double diff_centers(){	// ���� �߽ɰ��� ���ο� �߽ɰ����� ���� ��� �Լ�
	double diff= 0.0;

	for(int i=0;i<K;i++){
		for(int j=0;j<DIM;j++){
			diff += fabs(bcx[i][j] - cx[i][j]);
		}
	}

	return diff;
}

double JMSE(){ // ��� �ڽ� �������� ���ϴ� �Լ�
	double eachsum[K]={0}, totalsum=0.;
	double dis;
	for(int i=0;i<NOD;i++){

		dis= distance(data[i], data[i].label);
		
		eachsum[data[i].label] += dis;
	}

	for(int i=0;i<K;i++){
		totalsum += eachsum[i];
	}

	return totalsum;
}

int main(){

	int count=1;	// �ݺ� ȸ�� ī��Ʈ�� ����
	double diff = 0.0;	// ���� �߽ɰ��� ���� �߽ɰ� ���� ���� ����
	double jmse;	// ��� �ڽ� ������ ���� ����

	srand(int(time(NULL)));	// ���� �Լ� �õ� ����

	read_data();	// ������ �Է�

//	init_center_forgy();	// forgy ��� �ʱ�ȭ
	init_center_RP();		// ���� ���� ��� �ʱ�ȭ

	for(int i=0;i<K;i++){	// ���� �߽ɰ����� ���� �߽ɰ����� ����
		for(int j=0;j<DIM;j++){
			bcx[i][j] = cx[i][j];
		}
	}

	for (int i = 0; i<K; i++){	// �׽�Ʈ�� ��� ��ƾ
		for(int j=0; j<DIM; j++){
			printf("%.1f ", cx[i][j]);
		}
		cout << endl;
	}	

	assign_data();	// Ŭ�����ͺ� �ʱ� �߽ɰ��� �̿��Ͽ� �� ������ �Ҵ�

	for(int i=0;i<100;i++){
		update_centers();	// ���ο� �߽ɰ� ���

		jmse = JMSE();		// Jmse ���
		printf("Jmse: %.7f\n", jmse);

		diff = diff_centers();	// ���� �߽ɰ��� ���� ���

		printf("diff: %.7f\n",diff);

		if(diff < 0.0001){	// �߽ɺ����� ���̰��� ���� ������ �ݺ� ����
			break;
		}
		else{	//�׷��� ������ ���ο� �ݺ��� ���� ���� �߽ɰ��� �������� ����
			for(int i=0;i<K;i++){
				for(int j=0;j<DIM;j++){
					bcx[i][j] = cx[i][j];
				}
			}
		}

		cout << count++ << ":\n";

		for (int i = 0; i<K; i++){
			for(int j=0; j<DIM; j++){
				printf("%.1f ", cx[i][j]);
			}
			cout << endl;
		}

		assign_data();	// ���� ���� �߽ɰ����� ������ ����

	}

	print_data();		// ���� ��� �����͸� Ȯ���ϱ� ���� �κ�

	return 0;
}
