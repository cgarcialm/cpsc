import random 
import statistics 

def main():
    page_size = 2048
    n_tests = 100000 

    min_rand = 1 
    max_rand = 20000 
    list_internal_fragmentations = [] 
    for i in range(n_tests): 
        P = random.randint(min_rand, max_rand) 
        internal_fragmentation = page_size - (P % page_size) 
        list_internal_fragmentations.append(internal_fragmentation) 

    mean = statistics.mean(list_internal_fragmentations) 
    print("The mean of the internal fragmentation is:", mean)

if __name__ == "__main__":
    main()