#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <iostream>
#include <fstream>
#include <string.h>
#include <time.h>
#include <algorithm>

using namespace std;

#define NOD 150		// 데이터 개수
#define DIM 4		// 데이터 차원수
#define K 3			// 클러스터 수

struct XV{			//데이터 입력용 포맷
	double x[DIM];	// 데이터 값
	int real;		// 실제 분류값
	int label;		// 클러스터 결과값
};

XV data[NOD];		// 데이터 저장용 구조체배열
double cx[K][DIM];	// 클러스터 중심값 저장 배열
double bcx[K][DIM];  // 이전 클러스터 중심값 저장 배열

int fmin(double a[]){	// 최소값 찾는 함수
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

void read_data() {	// 파일로 부터 데이터를 읽어 구조체 배열에 저장
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
		cout << "파일을 찾을 수 없습니다!" << endl;
		exit(-1);
	}

	in.close();
}

void print_data() {	// 최종 결과 데이터를 클러스터 값과 함께 출력

	for (int i = 0; i<NOD; i++){
		for(int j=0; j<DIM; j++){
			printf("%.1f ", data[i].x[j]);
		}
		cout << data[i].real << ' ' << data[i].label << endl;
	}
}

void init_center_forgy(){	// forgy 알고리즘에 의한 초기 중심값 설정 함수

	int random;

	for(int i=0; i<K; i++){
		random = rand() % NOD;
		for(int j=0;j<DIM;j++){
			cx[i][j] = data[random].x[j];
		}
	}
}

void init_center_RP(){	//  random partition 기법에 의한 초기 중심값 설정 함수

	int random;
	double sum[K][DIM]={0};	// 데이터 값들의 합 저장
	int count[K]={0};		// 클러스터별 데이터 개수 저장

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

double distance(XV a, int k){	// k번째 클러스터와 a 데이터와의 거리 계산
	double diffsum = 0;
	for(int i=0;i<DIM;i++){
		diffsum += (a.x[i] - cx[k][i])*(a.x[i] - cx[k][i]);
	}

	return sqrt(diffsum);
}
	
void assign_data(){		
	// 클러스터 별 중심과의 거리가 가장 작은 쪽으로 클러스터 라벨을 할당하는 함수
	double dis[K]={0};

	for(int i=0;i<NOD;i++){

		for(int j=0;j<K;j++){
			dis[j]= distance(data[i], j);
		}
		data[i].label = fmin(dis);
	}
}

void update_centers(){	
	// 할당된 데이터들을 이용하여 새로운 중심값을 계산하는 함수

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

double diff_centers(){	// 이전 중심값과 새로운 중심값과의 차이 계산 함수
	double diff= 0.0;

	for(int i=0;i<K;i++){
		for(int j=0;j<DIM;j++){
			diff += fabs(bcx[i][j] - cx[i][j]);
		}
	}

	return diff;
}

double JMSE(){ // 평균 자승 오차값을 구하는 함수
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

	int count=1;	// 반복 회수 카운트용 변수
	double diff = 0.0;	// 이전 중심값과 현재 중심값 차이 저장 변수
	double jmse;	// 평균 자승 오차값 저장 변수

	srand(int(time(NULL)));	// 랜던 함수 시드 설정

	read_data();	// 데이터 입력

//	init_center_forgy();	// forgy 기법 초기화
	init_center_RP();		// 랜덤 분할 기법 초기화

	for(int i=0;i<K;i++){	// 현재 중심값들을 예전 중심값으로 복사
		for(int j=0;j<DIM;j++){
			bcx[i][j] = cx[i][j];
		}
	}

	for (int i = 0; i<K; i++){	// 테스트용 출력 루틴
		for(int j=0; j<DIM; j++){
			printf("%.1f ", cx[i][j]);
		}
		cout << endl;
	}	

	assign_data();	// 클러스터별 초기 중심값을 이용하여 각 데이터 할당

	for(int i=0;i<100;i++){
		update_centers();	// 새로운 중심값 계산

		jmse = JMSE();		// Jmse 계산
		printf("Jmse: %.7f\n", jmse);

		diff = diff_centers();	// 이전 중심과의 차이 계산

		printf("diff: %.7f\n",diff);

		if(diff < 0.0001){	// 중심벡터의 차이값이 아주 작으면 반복 종료
			break;
		}
		else{	//그렇지 않으면 새로운 반복을 위해 현재 중심값을 예전으로 복사
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

		assign_data();	// 새로 계산된 중심값으로 데이터 재배분

	}

	print_data();		// 최종 결과 데이터를 확인하기 위한 부분

	return 0;
}
