function [re, im] = Eigenvalues( A,B,n )
%EIGENVALUES
M = eig(A,B);
re = zeros(n);
im = zeros(n);
for i = 1:n
    re(i) = real(M(i));
    im(i) = imag(M(i));
end
end

