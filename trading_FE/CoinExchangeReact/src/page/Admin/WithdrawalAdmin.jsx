import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Button } from '@/components/ui/button';
import { Badge } from "@/components/ui/badge";
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Check, X } from 'lucide-react'; // Import icons

const WithdrawalAdmin = () => {
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const jwt = localStorage.getItem("jwt");
    const baseUrl = "http://localhost:8080/api";

    const fetchWithdrawals = async () => {
        setLoading(true);
        setError(null);
        if (!jwt) {
            setError(new Error("Bạn cần đăng nhập để thực hiện chức năng này."));
            setLoading(false);
            toast.error("Bạn cần đăng nhập!");
            return;
        }
        try {
            const response = await axios.get(`${baseUrl}/admin/withdrawal`, {
                headers: { Authorization: `Bearer ${jwt}` }
            });
            setRequests(response.data);
        } catch (err) {
            setError(err);
            console.error("Failed to fetch requests:", err);
            toast.error(`Không thể tải yêu cầu: ${err.message}`);
        } finally {
            setLoading(false);
        }
    };

    const handleProceed = async (id, accept) => {
        if (!jwt) {
            toast.error("Bạn cần đăng nhập!");
            return;
        }
        try {
            await axios.patch(`${baseUrl}/admin/withdrawal/${id}/proceed/${accept}`, null, {
                headers: { Authorization: `Bearer ${jwt}` }
            });
            toast.success(`Yêu cầu đã được ${accept ? 'chấp nhận' : 'từ chối'}!`);
            setRequests(prevRequests => prevRequests.filter(req => req.id !== id));
        } catch (err) {
            console.error("Failed to process withdrawal:", err.response?.data || err.message);
            toast.error(`Xử lý yêu cầu thất bại: ${err.response?.data?.message || err.message}`);
        }
    };

    useEffect(() => {
        fetchWithdrawals();
    }, []);

    

    const formatDate = (dateString) => {
        if (!dateString) return 'N/A';
        const options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' };
        return new Date(dateString).toLocaleDateString('vi-VN', options);
    };

    return (
      
        <div className="min-h-screen flex items-center justify-center bg-[#0D1117]">
            <div className="p-5 lg:px-20 w-full max-w-6xl h-screen"> {/* Giới hạn chiều rộng tối đa */}
                <h1 className="text-3xl font-bold py-5 text-center">Withdrawal Requests</h1>
                <Card className="w-full">
                    <CardHeader>
                        <CardTitle>Pending Withdrawals</CardTitle>
                        <CardDescription>
                            Review and process pending withdrawal requests.
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead className="w-[100px]">ID</TableHead>
                                    <TableHead>User ID / Email</TableHead> 
                                    <TableHead>Amount</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Date</TableHead>
                                    <TableHead className="text-right">Actions</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {loading && (
                                    <TableRow>
                                        <TableCell colSpan={6} className="text-center">Loading...</TableCell>
                                    </TableRow>
                                )}
                                {error && !loading && (
                                    <TableRow>
                                        <TableCell colSpan={6} className="text-center text-red-500">
                                            Error: {error.message}
                                        </TableCell>
                                    </TableRow>
                                )}
                                {!loading && !error && requests.length > 0 ? (
                                    requests.map((item) => (
                                        <TableRow key={item.id}>
                                            <TableCell className="font-medium">{item.id}</TableCell>
                                            <TableCell>{item.user?.email || 'N/A'}</TableCell> 
                                            <TableCell>{item.amount}</TableCell>
                                            <TableCell>
                                                <Badge variant={'outline'}> 
                                                    PENDING
                                                </Badge>
                                            </TableCell>
                                            <TableCell>{formatDate(item.date)}</TableCell>
                                            <TableCell className="text-right space-x-2">
                                                <Button 
                                                    variant="outline" 
                                                    size="sm" 
                                                    className="bg-green-600 hover:bg-green-700 text-white"
                                                    onClick={() => handleProceed(item.id, true)}
                                                >
                                                    <Check className="mr-2 h-4 w-4" />
                                                    Accept
                                                </Button>
                                                <Button 
                                                    variant="destructive" 
                                                    size="sm"
                                                    onClick={() => handleProceed(item.id, false)}
                                                >
                                                    <X className="mr-2 h-4 w-4" />
                                                    Decline
                                                </Button>
                                            </TableCell>
                                        </TableRow>
                                    ))
                                ) : !loading && !error && (
                                    <TableRow>
                                        <TableCell colSpan={6} className="text-center">No pending withdrawal requests found.</TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </CardContent>
                </Card>
                <ToastContainer position="top-right" autoClose={3000} hideProgressBar={false} />
            </div>
        </div>
    );
};

export default WithdrawalAdmin;